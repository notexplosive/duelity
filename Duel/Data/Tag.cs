namespace Duel.Data
{
    public abstract class Tag
    {
        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode();
        }
    }

    public class SpriteTag : Tag
    {
    }

    public class BlockProjectileTag : Tag
    {
    }

    public class SolidTag : Tag
    {
        public Type SolidType { get; }

        public enum Type
        {
            BlocksMovement,
            PushedOnBump // only makes sense on entities... I think?
        }

        public SolidTag(Type type)
        {
            SolidType = type;
        }
    }
}