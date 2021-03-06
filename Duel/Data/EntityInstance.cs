using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class EntityInstance
    {
        public Point Position { get; }
        public EntityTemplate Template { get; }
        public EntityInstance(Point position, EntityTemplate template)
        {
            Position = position;
            Template = template;
        }

        public override string ToString()
        {
            return $"{Template.NameInLibrary} instance at ({Position.X}, {Position.Y})";
        }
    }
}