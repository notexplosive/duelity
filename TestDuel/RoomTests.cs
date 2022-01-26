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
            this.level.LevelPosToRoomPos(new Point(-20, 0)).Should().Be(new Point(-2, 0));
            this.level.LevelPosToRoomPos(new Point(-10, 0)).Should().Be(new Point(-1, 0));

            this.level.LevelPosToRoomPos(new Point(0, 0)).Should().Be(new Point(0, 0));
            this.level.LevelPosToRoomPos(new Point(8, 5)).Should().Be(new Point(0, 0));

            this.level.LevelPosToRoomPos(new Point(20, 0)).Should().Be(new Point(1, 0));
            this.level.LevelPosToRoomPos(new Point(35, 0)).Should().Be(new Point(2, 0));
        }
    }
}
