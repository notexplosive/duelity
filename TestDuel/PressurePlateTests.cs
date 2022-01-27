using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class PressurePlateTests
    {
        private readonly Level level;
        private readonly EntityTemplate pressurePlateTemplate;

        public PressurePlateTests()
        {
            this.level = new Level(new Corners(new Point(-10, 10), new Point(10, 10)));
            this.pressurePlateTemplate = new EntityTemplate(new EnableSignalWhenSteppedOn(SignalColor.Red));
        }

        [Fact]
        public void defaults_not_pressed()
        {
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void pressed_when_solid_spawned_on_it()
        {
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);
            this.level.PutEntityAt(Point.Zero, new EntityTemplate(new Solid()));
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void pressed_when_solid_spawned_on_it_reversed_spawn_order()
        {
            this.level.PutEntityAt(Point.Zero, new EntityTemplate(new Solid()));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void pressed_when_player_walks_on_it()
        {
            var player = this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff)));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);

            this.level.SignalState.IsOn(SignalColor.Red).Should().BeFalse();
            player.WalkAndPushInDirection(Direction.Right);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();

        }

        [Fact]
        public void pressed_when_solid_pushed_on_it()
        {
            var pusher = this.level.PutEntityAt(new Point(-2, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff)));
            this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new Solid().PushOnBump()));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);

            this.level.SignalState.IsOn(SignalColor.Red).Should().BeFalse();
            pusher.WalkAndPushInDirection(Direction.Right);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void releases_when_solid_moves_off()
        {
            var solid = this.level.PutEntityAt(Point.Zero, new EntityTemplate(new Solid()));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);

            solid.WalkAndPushInDirection(Direction.Right);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void releases_when_solid_destroyed()
        {
            var solid = this.level.PutEntityAt(Point.Zero, new EntityTemplate(new Solid()));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);

            this.level.RemoveEntity(solid);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void maintain_signal_if_multiple_solids_moved_over()
        {
            var pusher = this.level.PutEntityAt(new Point(-2, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff)));
            this.level.PutEntityAt(new Point(-1, 0), new EntityTemplate(new Solid().PushOnBump()));
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);

            pusher.WalkAndPushInDirection(Direction.Right);
            pusher.WalkAndPushInDirection(Direction.Right);

            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void spawning_pressure_plate_should_not_affect_signal()
        {
            this.level.ForceSignalOn(SignalColor.Red);
            this.level.PutEntityAt(Point.Zero, this.pressurePlateTemplate);
            this.level.SignalState.IsOn(SignalColor.Red).Should().BeTrue();
        }
    }
}
