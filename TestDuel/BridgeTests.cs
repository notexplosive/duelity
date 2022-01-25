using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class BridgeTests
    {
        private readonly Level level;
        private readonly TileTemplate waterTemplate;
        private readonly TileTemplate bridgeTemplate;

        public BridgeTests()
        {
            this.level = new Level(new Corners(new Point(-10, 10), new Point(10, 10)));
            this.waterTemplate = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water), new Solid()); ;
            this.bridgeTemplate = new TileTemplate(new Collapses(waterTemplate));
        }

        [Fact]
        public void spawn_on_a_bridge_doesnt_break_it()
        {
            this.level.PutTileAt(Point.Zero, this.bridgeTemplate);
            this.level.PutEntityAt(Point.Zero, new EntityTemplate());

            this.level.GetTileAt(Point.Zero).Should().Be(this.bridgeTemplate);
        }

        [Fact]
        public void walking_on_a_bridge_doesnt_break_it()
        {
            this.level.PutTileAt(new Point(1, 0), this.bridgeTemplate);
            var entity = this.level.PutEntityAt(Point.Zero, new EntityTemplate());

            entity.WalkAndPushInDirection(Direction.Right);

            this.level.GetTileAt(new Point(1, 0)).Should().Be(this.bridgeTemplate);
        }

        [Fact]
        public void walking_off_a_bridge_breaks_it()
        {
            this.level.PutTileAt(Point.Zero, this.bridgeTemplate);
            var entity = this.level.PutEntityAt(Point.Zero, new EntityTemplate());

            entity.WalkAndPushInDirection(Direction.Right);

            this.level.GetTileAt(Point.Zero).Should().Be(this.waterTemplate);
        }
    }
}
