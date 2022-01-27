namespace Duel.Data
{
    public abstract class TemplateInstance
    {
        public abstract string TemplateName { get; }
        public abstract string CoordinateString { get; }
        public string TemplateClassName
        {
            get
            {
                switch (TemplateClass)
                {
                    case TemplateClass.Tile:
                        return "tile";
                    case TemplateClass.Entity:
                        return "entt";
                    case TemplateClass.Prop:
                        return "prop";
                }

                throw new System.Exception($"unknown template class {TemplateClass}");
            }
        }

        protected abstract TemplateClass TemplateClass { get; }
    }

    public enum TemplateClass
    {
        Tile,
        Entity,
        Prop
    }
}