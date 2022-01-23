using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class Direction
    {
        private Direction(string name, Point givenPoint)
        {
            this.name = name;
            this.internalPoint = givenPoint;
        }

        private readonly string name;
        private readonly Point internalPoint;
        public static Direction Up = new Direction("Up", new Point(0, -1));
        public static Direction Right = new Direction("Right", new Point(1, 0));
        public static Direction Down = new Direction("Down", new Point(0, 1));
        public static Direction Left = new Direction("Left", new Point(-1, 0));
        public static Direction None = new Direction("None", Point.Zero);

        public override string ToString() => this.name;

        public Point ToPoint()
        {
            return this.internalPoint;
        }

        public static Direction PointToDirection(Point point)
        {
            var absX = Math.Abs(point.X);
            var absY = Math.Abs(point.Y);
            if (absX > absY)
            {
                if (point.X < 0)
                {
                    return Left;
                }

                if (point.X > 0)
                {
                    return Right;
                }
            }

            if (absX < absY)
            {
                if (point.Y < 0)
                {
                    return Up;
                }

                if (point.Y > 0)
                {
                    return Down;
                }
            }

            return None;
        }

        public Direction Previous
        {
            get
            {
                if (this == Up)
                {
                    return Left;
                }

                if (this == Right)
                {
                    return Up;
                }

                if (this == Down)
                {
                    return Right;
                }

                if (this == Left)
                {
                    return Down;
                }

                return None;
            }
        }

        public Direction Next
        {
            get
            {
                if (this == Up)
                {
                    return Right;
                }

                if (this == Right)
                {
                    return Down;
                }

                if (this == Down)
                {
                    return Left;
                }

                if (this == Left)
                {
                    return Up;
                }

                return None;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   this.internalPoint.Equals(direction.internalPoint);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.internalPoint);
        }

        public static bool operator ==(Direction lhs, Direction rhs)
        {
            return lhs.internalPoint == rhs.internalPoint;
        }

        public static bool operator !=(Direction lhs, Direction rhs) => !(lhs == rhs);
    }
}
