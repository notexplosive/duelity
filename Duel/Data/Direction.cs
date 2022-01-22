using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class Direction
    {
        private Direction()
        {

        }

        public static Direction Up = new Direction();
        public static Direction Right = new Direction();
        public static Direction Down = new Direction();
        public static Direction Left = new Direction();
        public static Direction None = new Direction();

        public Point ToPoint()
        {
            if (this == Left)
            {
                return new Point(-1, 0);
            }

            if (this == Right)
            {
                return new Point(1, 0);
            }

            if (this == Down)
            {
                return new Point(0, 1);
            }

            if (this == Up)
            {
                return new Point(0, -1);
            }

            return Point.Zero;
        }

        public Direction PointToDirection(Point point)
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
    }
}
