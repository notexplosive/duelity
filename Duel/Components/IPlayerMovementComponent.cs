using Duel.Data;
using Microsoft.Xna.Framework;

namespace Duel.Components
{
    public interface IPlayerMovementComponent
    {
        void ResumeMoveFromOldInstance(Entity playerFromPreviousRoom, Point newPlayerPosition);
    }
}
