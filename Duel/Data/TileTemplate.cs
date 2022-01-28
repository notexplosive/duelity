namespace Duel.Data
{
    public class TileTemplate : ITemplate
    {
        public TagCollection Tags { get; } = new TagCollection();
        public string NameInLibrary { get; set; } = "empty_tile";

        public TileTemplate(params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                Tags.AddTag(tag);
            }
        }

        public TileTemplate(TagCollection tags)
        {
            foreach (var tag in tags)
            {
                Tags.AddTag(tag);
            }
        }
    }
}
