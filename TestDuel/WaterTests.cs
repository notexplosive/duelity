using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Xunit;

namespace TestDuel
{
    public class WaterTests
    {
        private readonly Level level;
        private readonly TileTemplate water;
        private readonly EntityTemplate sinkable;
        private readonly EntityTemplate floatable;

        public WaterTests()
        {
            this.level = new Level(new Corners(new Point(-10, 10), new Point(10, 10)));
            this.water = new TileTemplate(new Water());
            this.sinkable = new EntityTemplate(new WaterFiller(WaterFiller.Type.Sinks));
            this.floatable = new EntityTemplate(new WaterFiller(WaterFiller.Type.Floats));
        }

        [Fact]
        public void things_cannot_walk_on_water()
        {
            var walker = this.level.PutEntityAt(Point.Zero, new EntityTemplate());
            this.level.PutTileAt(new Point(1, 0), this.water);

            walker.WalkAndPushInDirection(Direction.Right);

            walker.Position.Should().Be(Point.Zero);
        }

        [Fact]
        public void floatable_can_walk_on_water()
        {
            var floater = this.level.PutEntityAt(Point.Zero, this.floatable);
            this.level.PutTileAt(new Point(1, 0), this.water);

            floater.WalkAndPushInDirection(Direction.Right);

            floater.Position.Should().Be(new Point(1, 0));
        }

        [Fact]
        public void floatable_is_replaced_when_on_water()
        {
            var floater = this.level.PutEntityAt(Point.Zero, this.floatable);
            this.level.PutTileAt(new Point(1, 0), this.water);
            var destroyedEntities = new List<Entity>();
            this.level.EntityDestroyRequested += (entity) =>
            {
                destroyedEntities.Add(entity);
            };

            floater.WalkAndPushInDirection(Direction.Right);

            destroyedEntities.Should().Contain(floater).And.HaveCount(1);
            this.level.GetTileAt(new Point(1, 0)).Tags.GetTag<Water>().FillingEntity.Should().Be(floater);
        }

        [Fact]
        public void filled_water_can_be_walked_on()
        {
            var walker = this.level.PutEntityAt(new Point(2, 0), new EntityTemplate());
            var floater = this.level.PutEntityAt(Point.Zero, this.floatable);
            this.level.PutTileAt(new Point(1, 0), this.water);

            floater.WalkAndPushInDirection(Direction.Right);
            walker.WalkAndPushInDirection(Direction.Left);

            walker.Position.Should().Be(new Point(1, 0));
        }

        [Fact]
        public void filled_water_does_not_consume_floatable()
        {
            var floater = this.level.PutEntityAt(Point.Zero, this.floatable);
            var secondFloater = this.level.PutEntityAt(new Point(2, 0), this.floatable);
            this.level.PutTileAt(new Point(1, 0), this.water);
            var destroyedEntities = new List<Entity>();
            this.level.EntityDestroyRequested += (entity) =>
            {
                destroyedEntities.Add(entity);
            };

            floater.WalkAndPushInDirection(Direction.Right);
            secondFloater.WalkAndPushInDirection(Direction.Left);

            destroyedEntities.Should().Contain(floater).And.HaveCount(1);
        }

        // sinkable_does_not_replace_water_tile
        // sinkable_is_destroyed_when_on_water
    }
}