using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class RoomTests
    {
        private readonly Level level;

        public RoomTests()
        {
            this.level = new Level();
        }

        [Fact]
        public void find_room_grid_pos()
        {
            Room.LevelPosToRoomPos(new Point(-1, -1)).Should().Be(new Point(-1, -1));

            Room.LevelPosToRoomPos(new Point(-20, 0)).Should().Be(new Point(-2, 0));
            Room.LevelPosToRoomPos(new Point(-10, 0)).Should().Be(new Point(-1, 0));

            Room.LevelPosToRoomPos(new Point(0, 0)).Should().Be(new Point(0, 0));
            Room.LevelPosToRoomPos(new Point(8, 5)).Should().Be(new Point(0, 0));

            Room.LevelPosToRoomPos(new Point(20, 0)).Should().Be(new Point(1, 0));
            Room.LevelPosToRoomPos(new Point(35, 0)).Should().Be(new Point(2, 0));
        }

        [Fact]
        public void get_bounds_for_room_zero()
        {
            var subject = new Room(new Point(0, 0));

            var bounds = subject.GetBounds();

            bounds.TopLeft.Should().Be(Point.Zero);
            bounds.BottomRight.Should().Be(Room.Size);

            Room.LevelPosToRoomPos(new Room(bounds.TopLeft).Position).Should().Be(Point.Zero);
        }

        [Fact]
        public void get_bounds_for_room_positive()
        {
            var subject = new Room(new Point(2, 3));

            var bounds = subject.GetBounds();

            bounds.TopLeft.Should().Be(new Point(Room.Size.X * 2, Room.Size.Y * 3));
            bounds.BottomRight.Should().Be(new Point(Room.Size.X * 2, Room.Size.Y * 3) + Room.Size);

            // confirm that our top left point is included in the room
            Room.LevelPosToRoomPos(new Room(bounds.TopLeft).Position).Should().Be(new Point(2, 3));
        }

        [Fact]
        public void get_bounds_for_room_at_negative_pos()
        {
            var subject = new Room(new Point(-1, -1));

            var bounds = subject.GetBounds();

            bounds.TopLeft.Should().Be(new Point(-15, -9));
            bounds.BottomRight.Should().Be(Point.Zero);

            // confirm that our top left point is included in the room
            Room.LevelPosToRoomPos(new Room(bounds.TopLeft).Position).Should().Be(new Point(-1, -1));
        }
    }
}
