using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class EntityInstance
    {
        public Point Position { get; }
        public TileTemplate Template { get; }
        public EntityInstance(Point position, TileTemplate template)
        {
            Position = position;
            Template = template;
        }
    }
}