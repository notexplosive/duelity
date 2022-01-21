using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public enum MoveType
    {
        Warp,
        Walk,
        Jump
    }

    public delegate void MoveAction(MoveType moveType, Point previousPosition);

    public class Entity
    {
        public static int UniqueIdPool = 0;
        public event MoveAction PositionChanged;

        public BusySignal BusySignal { get; } = new BusySignal();
        public TagCollection Tags { get; } = new TagCollection();

        private readonly int uniqueId;

        public Point Position { get; private set; }

        public Entity()
        {
            this.uniqueId = UniqueIdPool++;
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

        public void WalkToPosition(Point position)
        {
            var prevPosition = Position;
            Position = position;
            PositionChanged?.Invoke(MoveType.Walk, prevPosition);
        }
    }
}
