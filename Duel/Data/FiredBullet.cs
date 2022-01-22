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

                if (solidProvider.HasTagAt<Hittable>(hitScanPosition))
                {
                    HitLocations.Add(hitScanPosition);
                    HitAtLeastOneThing = true;
                }

                if (solidProvider.HasTagAt<BlockProjectileTag>(hitScanPosition))
                {
                    WasBlocked = true;
                    break;
                }
            }

            if (HitLocations.Count == 0)
            {
                HitLocations.Add(hitScanPosition);
            }
        }

        public bool HitAtLeastOneThing { get; }
        public Entity HittableEntity { get; }
        public bool WasBlocked { get; }
        public Point StartPosition { get; }
        public List<Point> HitLocations { get; } = new List<Point>();
    }
}
