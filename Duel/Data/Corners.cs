using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class Corners
    {
        public Corners(Point topLeft, Point bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public Point TopLeft { get; }
        public Point BottomRight { get; }

        public override bool Equals(object obj)
        {
            return obj is Corners corners &&
                   TopLeft.Equals(corners.TopLeft) &&
                   BottomRight.Equals(corners.BottomRight);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TopLeft.GetHashCode(), BottomRight.GetHashCode());
        }
    }
}
