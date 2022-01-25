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
            this.water = new TileTemplate(new UnfilledWater());
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
            this.level.EntityDestroyRequested += (entity, type) =>
            {
                destroyedEntities.Add(entity);
            };

            floater.WalkAndPushInDirection(Direction.Right);

            destroyedEntities.Should().Contain(floater).And.HaveCount(1);
            this.level.GetTileAt(new Point(1, 0)).Tags.GetTag<FilledWater>().FillingEntity.Should().Be(floater);
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
            this.level.EntityDestroyRequested += (entity, type) =>
            {
                destroyedEntities.Add(entity);
            };

            floater.WalkAndPushInDirection(Direction.Right);
            secondFloater.WalkAndPushInDirection(Direction.Left);

            destroyedEntities.Should().Contain(floater).And.HaveCount(1);
        }

        [Fact]
        public void sinkable_does_not_replace_tile()
        {
            var floater = this.level.PutEntityAt(Point.Zero, this.sinkable);
            this.level.PutTileAt(new Point(1, 0), this.water);
            var destroyedEntities = new List<Entity>();
            this.level.EntityDestroyRequested += (entity, type) =>
            {
                destroyedEntities.Add(entity);
            };

            floater.WalkAndPushInDirection(Direction.Right);

            destroyedEntities.Should().Contain(floater).And.HaveCount(1);
            this.level.GetTileAt(new Point(1, 0)).Tags.HasTag<UnfilledWater>().Should().BeTrue();
        }

        [Fact]
        public void filling_one_water_does_not_fill_all()
        {
            var floater = this.level.PutEntityAt(Point.Zero, this.floatable);
            this.level.PutTileAt(new Point(1, 0), this.water);
            this.level.PutTileAt(new Point(2, 0), this.water);

            floater.WalkAndPushInDirection(Direction.Right);

            this.level.GetTileAt(new Point(1, 0)).Tags.HasTag<FilledWater>().Should().BeTrue();
            this.level.GetTileAt(new Point(2, 0)).Tags.HasTag<UnfilledWater>().Should().BeTrue();
        }
    }
}