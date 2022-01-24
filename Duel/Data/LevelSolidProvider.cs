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
                if (entity.Tags.TryGetTag(out Solid solidTag))
                {
                    if (solidTag.IsPushOnBump)
                    {
                        entity.WalkAndPushInDirection(direction);
                    }
                    else
                    {
                        entity.Nudge(direction);
                    }
                }

                if (entity.Tags.TryGetTag(out ToggleSignal toggleSignal))
                {
                    if (toggleSignal.IsOnBump)
                    {
                        this.level.SignalState.Toggle(toggleSignal.Color);
                    }
                }
            }
        }

        public void BumpWithLassoAt(Point position)
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.TryGetTag(out ToggleSignal toggleSignal))
                {
                    if (toggleSignal.IsOnGrapple)
                    {
                        this.level.SignalState.Toggle(toggleSignal.Color);
                    }
                }
            }
        }

        public bool IsOutOfBounds(Point hitScanPosition)
        {
            return this.level.IsOutOfBounds(hitScanPosition);
        }

        public override bool IsSolidAt(Point position) // todo: rename to "IsNotWalkableAt"
        {
            if (IsOutOfBounds(position))
            {
                return true;
            }

            return HasTagAt<Solid>(position);
        }

        public void ApplyHitAt(Point hitLocation, Direction attackDirection)
        {
            var entities = new List<Entity>(this.level.AllEntitiesAt(hitLocation));
            foreach (var entity in entities)
            {
                if (entity.Tags.HasTag<DestroyOnHit>())
                {
                    this.level.RequestDestroyEntity(entity);
                }

                if (entity.Tags.TryGetTag(out Solid solid))
                {
                    if (solid.IsPushOnHit)
                    {
                        entity.WalkAndPushInDirection(attackDirection);
                    }
                }

                if (entity.Tags.TryGetTag(out ToggleSignal toggleSignal))
                {
                    if (toggleSignal.IsOnHit)
                    {
                        this.level.SignalState.Toggle(toggleSignal.Color);
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

        public bool TryGetFirstEntityWithTagAt<T>(Point position, out Entity foundEntity, out T foundTag) where T : Tag
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.TryGetTag(out T tag))
                {
                    foundEntity = entity;
                    foundTag = tag;
                    return true;
                }
            }

            foundEntity = null;
            foundTag = null;
            return false;
        }
    }
}
