using Microsoft.Xna.Framework;

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
            var offsetX = 0;
            if (levelGridPos.X < 0)
            {
                offsetX = -1;
            }

            var offsetY = 0;
            if (levelGridPos.Y < 0)
            {
                offsetY = -1;
            }

            return new Point(levelGridPos.X / Room.Size.X + offsetX, levelGridPos.Y / Room.Size.Y + offsetY);
        }
    }
}