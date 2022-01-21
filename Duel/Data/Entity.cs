using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class Entity
    {
        public static int UniqueIdPool = 0;
        private readonly int uniqueId;

        public Point Position { get; private set; }

        public Entity()
        {
            this.uniqueId = UniqueIdPool++;
        }

        public void WarpToPosition(Point position)
        {
            Position = position;
        }

        // Overrides //

        public override bool Equals(object obj)
        {
            if (obj is Entity other)
            {
                return other.uniqueId == this.uniqueId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.uniqueId;
        }

        public override string ToString()
        {
            return this.uniqueId.ToString();
        }
    }
}
