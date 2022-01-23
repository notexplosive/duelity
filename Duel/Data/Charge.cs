using Microsoft.Xna.Framework;

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
                RammedPosition = hitScanPosition + travelDirection.ToPoint();

                if (solidProvider.HasTagAt<SolidTag>(hitScanPosition))
                {
                    return;
                }

                hitScanPosition += travelDirection.ToPoint();
            }
        }

        public Point StartPosition { get; }
        public Point LandingPosition { get; }
        public Point RammedPosition { get; }
    }
}
