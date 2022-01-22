using Duel.Data;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestDuel
{
    public class FakeSolidProvider : SolidProvider
    {
        private readonly HashSet<Point> positions = new HashSet<Point>();

        public void BecomeSolidAt(Point position)
        {
            this.positions.Add(position);
        }

        public override bool IsSolidAt(Point position)
        {
            return this.positions.Contains(position);
        }
    }
}
