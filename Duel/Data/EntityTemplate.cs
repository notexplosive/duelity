namespace Duel.Data
{
    public class EntityTemplate
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
    }
}
