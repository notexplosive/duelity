using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Duel.Data
{
    public class Charge
    {
        public Charge(Point startPosition, Direction travelDirection, LevelSolidProvider solidProvider)
        {
            StartPosition = startPosition;
            var hitScanPosition = startPosition;

            while (!solidProvider.IsOutOfBounds(hitScanPosition))
            {
                LandingPosition = hitScanPosition;
                RammedPositions.Add(hitScanPosition + travelDirection.ToPoint());

                if (solidProvider.HasTagAt<SolidTag>(hitScanPosition))
                {
                    return;
                }

                hitScanPosition += travelDirection.ToPoint();
            }
        }

        public Point StartPosition { get; }
        public Point LandingPosition { get; }
        public List<Point> RammedPositions { get; } = new List<Point>();
    }
}
