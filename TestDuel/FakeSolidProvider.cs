using Duel.Data;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestDuel
{
    public class FakeSolidProvider : SolidProvider
    {
        private readonly HashSet<Point> positions = new HashSet<Point>();

        public override void ApplyPushAt(Point position, Direction direction)
        {
        }

        public void BecomeSolidAt(Point position)
        {
            this.positions.Add(position);
        }

        public override bool IsNotWalkableAt(Entity walker, Point position)
        {
            return this.positions.Contains(position);
        }
    }
}
