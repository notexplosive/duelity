using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class EmptySolidProvider : SolidProvider
    {
        public override void ApplyPushAt(Point position, Direction direction)
        {
        }

        public override bool IsSolidAt(Point position)
        {
            return false;
        }
    }
}
