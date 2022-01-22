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
                }
            }
        }

        public override bool IsSolidAt(Point position)
        {
            if (this.level.IsOutOfBounds(position))
            {
                return true;
            }

            return HasTagAt<SolidTag>(position);
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
