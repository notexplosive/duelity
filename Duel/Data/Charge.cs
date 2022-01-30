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
            var currentPosition = startPosition;

            while (!solidProvider.IsOutOfBounds(currentPosition))
            {
                Path.Add(new ChargeHit(currentPosition, travelDirection));

                var nextPos = currentPosition + travelDirection.ToPoint();

                if (solidProvider.IsClosedDoorAt(nextPos))
                {
                    return;
                }

                if (solidProvider.IsRavineAt(currentPosition))
                {
                    return;
                }

                if (solidProvider.HasTagAt<Solid>(nextPos))
                {
                    var hitSolidEntity = solidProvider.IsEntityWithTagAt<Solid>(nextPos) && !solidProvider.IsEntityWithTagAt<DestroyOnHit>(nextPos);
                    var hitSolidTile = solidProvider.TryGetTagFromTileAt(nextPos, out Solid foundSolid);

                    if (hitSolidEntity || hitSolidTile)
                    {
                        return;
                    }
                }

                currentPosition = nextPos;
            }
        }

        public Direction Direction { get; }
        public Point StartPosition { get; }
        public List<ChargeHit> Path { get; } = new List<ChargeHit>();
    }
}
