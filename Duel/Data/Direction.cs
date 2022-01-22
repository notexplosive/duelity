using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class Direction
    {
        private Direction(Point givenPoint)
        {
            this.internalPoint = givenPoint;
        }

        private readonly Point internalPoint;
        public static Direction Up = new Direction(new Point(0, -1));
        public static Direction Right = new Direction(new Point(1, 0));
        public static Direction Down = new Direction(new Point(0, 1));
        public static Direction Left = new Direction(new Point(-1, 0));
        public static Direction None = new Direction(Point.Zero);

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
    }
}
