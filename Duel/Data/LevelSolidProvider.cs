using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public class LevelSolidProvider : SolidProvider
    {
        private readonly Level level;

        public LevelSolidProvider(Level level)
        {
            this.level = level;
        }

        public override void ApplyPushAt(Point position, Direction direction)
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.TryGetTag(out SolidTag solidTag))
                {
                    if (solidTag.SolidType == SolidTag.Type.Pushable)
                    {
                        entity.WalkAndPushInDirection(direction);
                    }
                    else
                    {
                        entity.Nudge(direction);
                    }
                }
            }
        }

        public bool IsOutOfBounds(Point hitScanPosition)
        {
            return this.level.IsOutOfBounds(hitScanPosition);
        }

        public override bool IsSolidAt(Point position)
        {
            if (IsOutOfBounds(position))
            {
                return true;
            }

            return HasTagAt<SolidTag>(position);
        }

        public void ApplyHitAt(Point hitLocation, Direction attackDirection)
        {
            var entities = new List<Entity>(this.level.AllEntitiesAt(hitLocation));
            foreach (var entity in entities)
            {
                if (entity.Tags.TryGetTag(out Hittable hittable))
                {
                    if (hittable.HitResponseType == Hittable.Type.DestroyOnHit)
                    {
                        this.level.RequestDestroyEntity(entity);
                    }

                    if (hittable.HitResponseType == Hittable.Type.PushOnHit)
                    {
                        entity.WalkAndPushInDirection(attackDirection);
                    }
                }
            }
        }

        public bool HasTagAt<T>(Point position) where T : Tag
        {
            if (this.level.GetTileAt(position).Tags.HasTag<T>())
            {
                return true;
            }

            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.HasTag<T>())
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetFirstEntityWithTagAt<T>(Point position, out Entity result) where T : Tag
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.HasTag<T>())
                {
                    result = entity;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
