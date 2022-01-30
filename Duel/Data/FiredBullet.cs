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

            while (!solidProvider.IsOutOfBoundsOfCurrentRoomOrLevel(position, hitScanPosition))
            {
                hitScanPosition += shootDirection.ToPoint();

                if (solidProvider.HasTagAt<DestroyOnHit>(hitScanPosition))
                {
                    HitLocations.Add(hitScanPosition);
                    HitAtLeastOneThing = true;
                }

                if (solidProvider.TryGetFirstEntityWithTagAt(hitScanPosition, out Entity foundEntity, out Solid solid))
                {
                    if (solid.IsPushOnHit)
                    {
                        solid.PlayPushSound();
                        HitLocations.Add(hitScanPosition);
                        HitAtLeastOneThing = true;
                    }
                }

                if (solidProvider.BlocksBulletsAt(position, hitScanPosition))
                {
                    HitAtLeastOneThing = true;
                    WasBlocked = true;
                    break;
                }
            }

            if (!HitLocations.Contains(hitScanPosition))
            {
                HitLocations.Add(hitScanPosition);
            }
        }

        public IEnumerable<Point> HitLocationsReversed()
        {
            var result = new List<Point>(HitLocations);
            result.Reverse();
            return result;
        }

        public bool HitAtLeastOneThing { get; }
        public Entity HittableEntity { get; }
        public bool WasBlocked { get; }
        public Point StartPosition { get; }
        public List<Point> HitLocations { get; } = new List<Point>();
        public Point LastHitLocation
        {
            get
            {
                foreach (var location in HitLocationsReversed())
                {
                    return location;
                }

                return StartPosition;
            }
        }
    }
}
