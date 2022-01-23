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
            level.PutEntityAt(new Point(1, 1), new EntityTemplate(new Solid()));

            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(1, 1)).Should().BeTrue();
        }

        [Fact]
        public void entity_walking_into_pushable_pushes()
        {
            var level = new Level(new Corners(new Point(-3, -3), new Point(3, 3)));
            var pushable = level.PutEntityAt(new Point(1, 1), new EntityTemplate(new Solid().PushOnBump()));
            var pusher = level.PutEntityAt(new Point(1, 2), new EntityTemplate());

            pusher.WalkAndPushInDirection(Direction.Up);

            pushable.Position.Should().Be(new Point(1, 0));
        }

        [Fact]
        public void a_pushes_b_pushes_c_pushes_d()
        {
            var level = new Level(new Corners(new Point(-10, -10), new Point(10, 10)));
            var pusher = level.PutEntityAt(new Point(0, 0), new EntityTemplate());
            var a = level.PutEntityAt(new Point(1, 0), new EntityTemplate(new Solid().PushOnBump()));
            var b = level.PutEntityAt(new Point(2, 0), new EntityTemplate(new Solid().PushOnBump()));
            var c = level.PutEntityAt(new Point(3, 0), new EntityTemplate(new Solid().PushOnBump()));
            var d = level.PutEntityAt(new Point(4, 0), new EntityTemplate(new Solid().PushOnBump()));

            pusher.WalkAndPushInDirection(Direction.Right);

            a.Position.Should().Be(new Point(2, 0));
            b.Position.Should().Be(new Point(3, 0));
            c.Position.Should().Be(new Point(4, 0));
            d.Position.Should().Be(new Point(5, 0));
        }

        [Fact]
        public void hit_at_location_to_push()
        {
            var level = new Level(new Corners(new Point(-10, -10), new Point(10, 10)));
            var pushable = level.PutEntityAt(Point.Zero, new EntityTemplate(new BlockProjectileTag(), new Solid().PushOnHit()));
            var solidProvider = new LevelSolidProvider(level);

            solidProvider.ApplyHitAt(Point.Zero, Direction.Right);
            pushable.Position.Should().Be(new Point(1, 0));
        }
    }
}
