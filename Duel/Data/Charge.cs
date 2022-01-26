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

                var nextPos = hitScanPosition + travelDirection.ToPoint();
                if (solidProvider.HasTagAt<Solid>(nextPos))
                {
                    var hitSolidEntity = solidProvider.IsEntityWithTagAt<Solid>(nextPos) && !solidProvider.IsEntityWithTagAt<DestroyOnHit>(nextPos);
                    var hitSolidTile = solidProvider.TryGetTagFromTileAt(nextPos, out Solid foundSolid);

                    if (hitSolidEntity || hitSolidTile)
                    {
                        return;
                    }
                }

                hitScanPosition = nextPos;
            }
        }

        public Direction Direction { get; }
        public Point StartPosition { get; }
        public List<ChargeHit> Path { get; } = new List<ChargeHit>();
    }
}
