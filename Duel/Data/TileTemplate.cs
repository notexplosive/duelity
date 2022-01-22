namespace Duel.Data
{
    public class TileTemplate
    {
        public TagCollection Tags { get; } = new TagCollection();

        public TileTemplate(params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                Tags.AddTag(tag);
            }
        }
    }
}
