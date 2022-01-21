using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    public class DirectionalButtons
    {
        public bool up;
        public bool right;
        public bool down;
        public bool left;

        public bool HasPressed()
        {
            return this.up || this.right || this.down || this.left;
        }

        public Direction GetFirstDirection()
        {
            if (this.up && !this.down) { return Direction.Up; }
            if (this.down && !this.up) { return Direction.Down; }

            if (this.left && !this.right) { return Direction.Left; }
            if (this.right && !this.left) { return Direction.Right; }

            return Direction.None;
        }
    }
}
