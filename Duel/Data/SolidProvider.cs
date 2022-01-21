using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public abstract class SolidProvider
    {
        public abstract bool IsSolidAt(Point position);
    }
}
