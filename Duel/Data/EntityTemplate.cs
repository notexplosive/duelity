namespace Duel.Data
{
    public class EntityTemplate : ITemplate
    {
        private readonly Tag[] tags;

        public EntityTemplate(params Tag[] tags)
        {
            this.tags = tags;
        }

        public Entity Create(LevelSolidProvider provider)
        {
            var entity = new Entity(provider);

            foreach (var tag in tags)
            {
                entity.Tags.AddTag(tag);
            }

            return entity;
        }

        public TagCollection Tags
        {
            get
            {
                var tagCollection = new TagCollection();
                foreach (var tag in tags)
                {
                    tagCollection.AddTag(tag);
                }
                return tagCollection;
            }
        }
    }
}
