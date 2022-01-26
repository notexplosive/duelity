using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public abstract class SolidProvider
    {
        public abstract bool IsNotWalkableAt(Entity walker, Point position);

        public abstract void ApplyPushAt(Point position, Direction direction);
    }
}
