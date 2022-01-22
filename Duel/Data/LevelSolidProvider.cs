using Machina.Engine;
using Microsoft.Xna.Framework;

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
            if (this.level.GetTileAt(position).Tags.HasTag<SolidTag>())
            {
                return true;
            }

            if (this.level.IsOutOfBounds(position))
            {
                return true;
            }

            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                if (entity.Tags.HasTag<SolidTag>())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
