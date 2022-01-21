using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class LevelSolidProviderTests
    {
        [Fact]
        public void empty_space_is_not_solid_if_in_bounds()
        {
            var level = new Level();
            level.PutTileAt(new Point(-2, -2), new TileTemplate());
            level.PutTileAt(new Point(2, 2), new TileTemplate());
            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(0, 0)).Should().BeFalse();
        }

        [Fact]
        public void empty_space_is_solid_if_out_of_bounds()
        {
            var level = new Level();
            level.PutTileAt(new Point(-2, -2), new TileTemplate());
            level.PutTileAt(new Point(2, 2), new TileTemplate());
            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(10, 10)).Should().BeTrue();
        }

        [Fact]
        public void solid_entities_are_solid()
        {
            var level = new Level();
            var entity = new Entity();
            level.PutTileAt(new Point(-2, -2), new TileTemplate());
            level.PutTileAt(new Point(2, 2), new TileTemplate());
            entity.Tags.AddTag(new SolidTag(SolidTag.Type.Static));
            entity.WarpToPosition(new Point(1, 1));
            level.AddEntity(entity);

            var solidProvider = new LevelSolidProvider(level);

            solidProvider.IsSolidAt(new Point(1, 1)).Should().BeTrue();
        }
    }
}
