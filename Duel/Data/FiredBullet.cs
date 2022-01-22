using Duel.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class FiredBullet
    {
        public FiredBullet(Point position, Direction shootDirection, LevelSolidProvider solidProvider)
        {
            StartPosition = position;
            var hitScanPosition = position;

            while (!solidProvider.IsOutOfBounds(hitScanPosition))
            {
                hitScanPosition += shootDirection.ToPoint();

                bool stop = false;

                if (solidProvider.HasTagAt<Hittable>(hitScanPosition))
                {
                    HitSomethingHittable = true;
                    stop = true;
                }

                if (solidProvider.HasTagAt<BlockProjectileTag>(hitScanPosition))
                {
                    WasBlocked = true;
                    stop = true;
                }

                if (stop)
                    break;
            }

            HitLocation = hitScanPosition;
        }

        public bool HitSomethingHittable { get; }
        public Entity HittableEntity { get; }
        public bool WasBlocked { get; }
        public Point HitLocation { get; }
        public Point StartPosition { get; }
    }
}
