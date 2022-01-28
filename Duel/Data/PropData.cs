using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class PropData
    {
        public Point Position { get; }
        public PropTemplate Template { get; }
        public PropData(Point position, PropTemplate template)
        {
            Position = position;
            Template = template;
        }
    }
}