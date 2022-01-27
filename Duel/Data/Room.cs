using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class Room
    {
        public static Point Size = new Point(15, 9);

        public Room(Point roomPos)
        {
            Position = roomPos;
        }

        public Point Position { get; }

        public Corners GetBounds()
        {
            var topLeft = new Point(Position.X * Size.X, Position.Y * Size.Y);
            return new Corners(topLeft, topLeft + Size);
        }

        public static Point LevelPosToRoomPos(Point levelGridPos)
        {
            return new Point((int)MathF.Floor((float)levelGridPos.X / Room.Size.X), (int)MathF.Floor((float)levelGridPos.Y / Room.Size.Y));
        }

        public override bool Equals(object obj)
        {
            return obj is Room room &&
                   Position.Equals(room.Position);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Position);
        }
    }
}