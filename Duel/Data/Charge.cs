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
                    if (solidProvider.TryGetFirstEntityWithTagAt(hitScanPosition + travelDirection.ToPoint(), out Entity foundEntity, out Solid solid))
                    {
                        if (!foundEntity.Tags.HasTag<DestroyOnHit>() /* or is water */)
                        {
                            return; // this will have weird behavior if we charge onto a glass object that's also inside a wall
                        }
                    }
                    else
                    {
                        // there was only a solid tile there, return
                        return;
                    }
                }

                hitScanPosition += travelDirection.ToPoint();
            }
        }

        public Direction Direction { get; }
        public Point StartPosition { get; }
        public List<ChargeHit> Path { get; } = new List<ChargeHit>();
    }
}
