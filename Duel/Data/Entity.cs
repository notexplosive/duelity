using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public enum MoveType
    {
        Warp,
        Walk,
        Charge,
        Jump,
        Spawn
    }

    public delegate void MoveAction(Entity mover, MoveType moveType, Point previousPosition);
    public delegate void BumpAction(Entity entity, Point position, Direction direction);

    public delegate void DirectionalAction(Direction direction);

    public class Entity : TemplateInstance
    {
        public static int UniqueIdPool = 0;

        public event MoveAction PositionChanged;
        public event DirectionalAction MoveFailed;
        public event DirectionalAction Nudged;
        public event BumpAction Bumped;
        public event Action GrabbedByLasso;
        public event Action ReleasedFromLasso;
        public event Action<EaseFunc, Point> Jumped;

        public BusySignal BusySignal { get; } = new BusySignal();
        public TagCollection Tags { get; } = new TagCollection();

        private readonly int uniqueId;

        public Point Position { get; private set; }
        public Direction FacingDirection { get; set; } = Direction.Down;
        public SolidProvider SolidProvider { get; }

        public void Nudge(Direction direction)
        {
            // Purely graphical
            Nudged?.Invoke(direction);
        }

        public void GrabWithLasso()
        {
            GrabbedByLasso?.Invoke();
        }

        public void ReleaseFromLasso()
        {
            ReleasedFromLasso?.Invoke();
        }

        public Entity()
        {
            this.uniqueId = UniqueIdPool++;
            SolidProvider = new EmptySolidProvider();
        }

        public Entity(SolidProvider solidProvider, string templateName) : this()
        {
            SolidProvider = solidProvider;
            TemplateName = templateName;
        }

        // Overrides //

        public override bool Equals(object obj)
        {
            if (obj is Entity other)
            {
                return other.uniqueId == this.uniqueId;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.uniqueId;
        }

        public override string ToString()
        {
            return $"Entity {this.uniqueId}: {Tags}";
        }

        // /Overrides //

        public void WarpToPosition(Point position)
        {
            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(this, MoveType.Warp, prevPosition);
        }

        public void ChargeToPosition(Point position, Direction direction)
        {
            FacingDirection = direction;

            if (Position == position)
            {
                return;
            }

            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(this, MoveType.Charge, prevPosition);
        }

        public void JumpToPosition(Point position, EaseFunc easeFunc = null)
        {
            if (Position == position)
            {
                return;
            }

            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(this, MoveType.Jump, prevPosition);

            if (easeFunc == null)
            {
                easeFunc = EaseFuncs.EaseInBack;
            }

            Jumped?.Invoke(easeFunc, prevPosition);
        }

        public void WalkWithoutPushInDirection(Direction direction) // dead code???
        {
            FacingDirection = direction;

            if (SolidProvider.IsNotWalkableAt(this, Position + direction.ToPoint()))
            {
                // Bumped???
                MoveFailed?.Invoke(direction);
                return;
            }

            var prevPosition = Position;
            Position += direction.ToPoint();
            PositionChanged?.Invoke(this, MoveType.Walk, prevPosition);
        }

        public void WalkAndPushInDirection(Direction direction)
        {
            FacingDirection = direction;

            if (SolidProvider.IsNotWalkableAt(this, Position + direction.ToPoint()))
            {
                Bumped?.Invoke(this, Position + direction.ToPoint(), direction);
                SolidProvider.ApplyPushAt(Position + direction.ToPoint(), direction);

                // If it's still solid, give up, otherwise we move
                if (SolidProvider.IsNotWalkableAt(this, Position + direction.ToPoint()))
                {
                    MoveFailed?.Invoke(direction);
                    return;
                }
            }

            var prevPosition = Position;
            Position += direction.ToPoint();
            PositionChanged?.Invoke(this, MoveType.Walk, prevPosition);
        }

        protected override TemplateClass TemplateClass => TemplateClass.Entity;

        public override string TemplateName { get; }

        public override string CoordinateString
        {
            get
            {
                var pos = Position;
                return $"{pos.X},{pos.Y}";
            }
        }
    }
}