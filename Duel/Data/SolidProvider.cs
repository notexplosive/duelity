using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public abstract class SolidProvider
    {
        public abstract bool IsNotWalkableAt(Point position);

        public abstract void ApplyPushAt(Point position, Direction direction);
    }
}
