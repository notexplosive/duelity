using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class SignalDoorTests
    {
        private readonly Level level;
        private readonly EntityTemplate openDoor;
        private readonly EntityTemplate closedDoor;

        public SignalDoorTests()
        {
            this.level = new Level(new Corners(new Point(-10, 10), new Point(10, 10)));
            this.openDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, true));
            this.closedDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, false));
        }

        [Fact]
        public void entities_cannot_walk_through_closed_doors()
        {
            this.level.PutEntityAt(Point.Zero, this.closedDoor);
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate());
            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(-1, 0));
        }

        [Fact]
        public void entities_can_walk_through_opened_doors()
        {
            this.level.PutEntityAt(Point.Zero, this.openDoor);
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate());
            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void entity_can_walk_through_closed_door_if_signaled_open()
        {
            this.level.PutEntityAt(Point.Zero, this.closedDoor);
            this.level.SignalState.TurnOn(SignalColor.Red);
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate());
            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void entity_cannot_walk_through_opened_door_if_signaled_close()
        {
            this.level.PutEntityAt(Point.Zero, this.openDoor);
            this.level.SignalState.TurnOn(SignalColor.Red);
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate());
            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(-1, 0));
        }

        [Fact]
        public void entity_can_walk_into_closing_door()
        {
            this.level.PutEntityAt(Point.Zero, this.closedDoor);
            this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new EnableSignalWhenSteppedOn(SignalColor.Red))); // pressure plate just under the entity
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new Solid())); // entity is solid so it can press the plate

            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue(); // fail fast, if pressure plates are broken this test is broken

            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void entity_cannot_walk_into_opening_door()
        {
            this.level.PutEntityAt(Point.Zero, this.openDoor);
            this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new EnableSignalWhenSteppedOn(SignalColor.Red))); // pressure plate just under the entity
            var walker = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new Solid())); // entity is solid so it can press the plate

            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue(); // fail fast, if pressure plates are broken this test is broken

            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(new Point(-1, 0));
        }
    }
}
