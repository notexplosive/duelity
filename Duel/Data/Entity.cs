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
        Jump
    }

    public delegate void MoveAction(MoveType moveType, Point previousPosition);
    public delegate void DirectionalAction(Direction direction);

    public class Entity
    {
        public static int UniqueIdPool = 0;

        public event MoveAction PositionChanged;
        public event DirectionalAction MoveFailed;
        public event DirectionalAction Nudged;
        public event Action<EaseFunc, Point> Jumped;

        public BusySignal BusySignal { get; } = new BusySignal();
        public TagCollection Tags { get; } = new TagCollection();

        private readonly int uniqueId;

        public Point Position { get; private set; }
        public Direction FacingDirection { get; private set; } = Direction.Down;
        public SolidProvider SolidProvider { get; }

        public void Nudge(Direction direction)
        {
            // Purely graphical
            Nudged?.Invoke(direction);
        }

        public Entity()
        {
            this.uniqueId = UniqueIdPool++;
            SolidProvider = new EmptySolidProvider();
        }

        public Entity(SolidProvider solidProvider) : this()
        {
            SolidProvider = solidProvider;
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
            return this.uniqueId.ToString();
        }

        // /Overrides //

        public void WarpToPosition(Point position)
        {
            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(MoveType.Warp, prevPosition);
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
            PositionChanged?.Invoke(MoveType.Charge, prevPosition);
        }

        public void JumpToPosition(Point position, EaseFunc easeFunc = null)
        {
            if (Position == position)
            {
                return;
            }

            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(MoveType.Jump, prevPosition);

            if (easeFunc == null)
            {
                easeFunc = EaseFuncs.EaseInBack;
            }
            Jumped?.Invoke(easeFunc, prevPosition);
        }

        public void WalkWithoutPushInDirection(Direction direction)
        {
            FacingDirection = direction;

            if (this.SolidProvider.IsSolidAt(Position + direction.ToPoint()))
            {
                MoveFailed?.Invoke(direction);
                return;
            }

            var prevPosition = Position;
            Position += direction.ToPoint();
            PositionChanged?.Invoke(MoveType.Walk, prevPosition);
        }

        public void WalkAndPushInDirection(Direction direction)
        {
            FacingDirection = direction;

            if (this.SolidProvider.IsSolidAt(Position + direction.ToPoint()))
            {
                SolidProvider.ApplyPushAt(Position + direction.ToPoint(), direction);

                // If it's still solid, give up, otherwise we move
                if (this.SolidProvider.IsSolidAt(Position + direction.ToPoint()))
                {
                    MoveFailed?.Invoke(direction);
                    return;
                }
            }

            var prevPosition = Position;
            Position += direction.ToPoint();
            PositionChanged?.Invoke(MoveType.Walk, prevPosition);
        }
    }
}
