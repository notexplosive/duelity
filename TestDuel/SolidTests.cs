using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class SolidTests
    {
        [Fact]
        public void empty_space_is_not_solid_if_in_bounds()
        {
            var level = new Level(new Corners(new Point(-2, -2), new Point(2, 2)));
            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(0, 0)).Should().BeFalse();
        }

        [Fact]
        public void empty_space_is_solid_if_out_of_bounds()
        {
            var level = new Level(new Corners(new Point(-2, -2), new Point(2, 2)));
            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(10, 10)).Should().BeTrue();
        }

        [Fact]
        public void solid_entities_are_solid()
        {
            var level = new Level(new Corners(new Point(-2, -2), new Point(2, 2)));
            level.CreateEntityWithTags(new Point(1, 1), new SolidTag(SolidTag.Type.Static));

            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(1, 1)).Should().BeTrue();
        }

        [Fact]
        public void entity_walking_into_pushable_pushes()
        {
            var level = new Level(new Corners(new Point(-3, -3), new Point(3, 3)));
            var pushable = level.CreateEntityWithTags(new Point(1, 1), new SolidTag(SolidTag.Type.Pushable));
            var pusher = level.CreateEntityWithTags(new Point(1, 2));

            pusher.WalkInDirection(Direction.Up);

            pushable.Position.Should().Be(new Point(1, 0));
        }
    }
}
