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
                        toggleSignal.Toggle();
                        this.level.UpdateSignalState();
                    }
                }
            }
        }

        public bool IsEntityWithTagAt<T>(Point position) where T : Tag
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.TryGetTag(out T tag))
                {
                    return true;
                }
            }

            return false;
        }

        public bool BlocksBulletsAt(Point position)
        {
            return HasTagAt<BlockProjectileTag>(position) || IsClosedDoorAt(position);
        }

        public bool TryGetTagFromTileAt<T>(Point position, out T foundTileTag) where T : Tag
        {
            if (this.level.GetTileAt(position).Tags.TryGetTag(out T result))
            {
                foundTileTag = result;
                return true;
            }

            foundTileTag = null;
            return false;
        }

        public bool IsWaterAt(Point position)
        {
            if (HasTagAt<UnfilledWater>(position))
            {
                return true;
            }

            return false;
        }

        public bool IsRavineAt(Point position)
        {
            if (HasTagAt<Ravine>(position))
            {
                return true;
            }

            return false;
        }

        public bool IsClosedDoorAt(Point position)
        {
            if (TryGetFirstEntityWithTagAt(position, out Entity foundEntity, out SignalDoor door))
            {
                var signalIsOffAndStartClosed = !this.level.SignalState.IsOn(door.Color) && !door.DefaultOpened;
                var signalIsOnAndStartOpened = this.level.SignalState.IsOn(door.Color) && door.DefaultOpened;
                return signalIsOffAndStartClosed || signalIsOnAndStartOpened;
            }

            return false;
        }

        public void BumpWithLassoAt(Point position)
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.TryGetTag(out ToggleSignal toggleSignal))
                {
                    if (toggleSignal.IsOnGrapple)
                    {
                        toggleSignal.Toggle();
                        this.level.UpdateSignalState();
                    }
                }
            }
        }

        public bool IsOutOfBounds(Point hitScanPosition)
        {
            return this.level.IsOutOfBounds(hitScanPosition);
        }

        public override bool IsNotWalkableAt(Entity walker, Point position)
        {
            if (IsOutOfBounds(position))
            {
                return true;
            }

            if (IsClosedDoorAt(position))
            {
                return true;
            }

            if (IsWaterAt(position) && !walker.Tags.HasTag<WaterFiller>())
            {
                return true;
            }

            if (IsRavineAt(position) && !walker.Tags.HasTag<WaterFiller>())
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
                    this.level.RequestDestroyEntity(entity, DestroyType.Break);
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
                        toggleSignal.Toggle();
                        this.level.UpdateSignalState();
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
