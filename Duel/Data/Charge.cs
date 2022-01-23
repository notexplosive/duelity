using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Duel.Data
{
    public class Charge
    {
        public Charge(Point startPosition, Direction travelDirection, LevelSolidProvider solidProvider)
        {
            Direction = travelDirection;
            StartPosition = startPosition;
            var hitScanPosition = startPosition;

            while (!solidProvider.IsOutOfBounds(hitScanPosition))
            {
                Path.Add(new ChargeHit(hitScanPosition, travelDirection));

                if (solidProvider.HasTagAt<Solid>(hitScanPosition + travelDirection.ToPoint()))
                {
                    return;
                }

                hitScanPosition += travelDirection.ToPoint();
            }
        }

        public Direction Direction { get; }
        public Point StartPosition { get; }
        public List<ChargeHit> Path { get; } = new List<ChargeHit>();
    }
}
