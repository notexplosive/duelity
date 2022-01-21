using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class EmptySolidProvider : SolidProvider
    {
        public override bool IsSolidAt(Point position)
        {
            return false;
        }
    }
}
