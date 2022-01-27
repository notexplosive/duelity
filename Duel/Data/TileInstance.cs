using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class TileInstance : TemplateInstance
    {
        public Point Position { get; }
        public TileTemplate Template { get; }

        public override string TemplateName => Template.NameInLibrary;

        protected override TemplateClass TemplateClass => TemplateClass.Tile;

        public override string CoordinateString
        {
            get
            {
                var pos = Position;
                return $"{pos.X},{pos.Y}";
            }
        }

        public TileInstance(Point position, TileTemplate template)
        {
            Position = position;
            Template = template;
        }
    }
}