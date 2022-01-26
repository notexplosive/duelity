using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class AutoTileTests
    {
        [Fact]
        public void get_auto_tile_from_level()
        {
            var level = new Level(new Corners(new Point(-10, -10), new Point(10, 10)));
            var water = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water));
            level.PutTileAt(new Point(5, 5), water);
            level.PutTileAt(new Point(7, 3), water);
            var autoTile = level.ComputeAutoTile(TileImageTag.TileImage.Water);

            autoTile.ExistsAt(new Point(5, 5)).Should().BeTrue();
            autoTile.ExistsAt(new Point(7, 3)).Should().BeTrue();
            autoTile.ExistsAt(new Point(8, 4)).Should().BeFalse();
        }

        [Fact]
        public void detect_no_tile()
        {
            var subject = new AutoTile();
            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.None);
        }

        [Fact]
        public void lone_tile_is_lone()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.Alone);
        }

        [Fact]
        public void place_same_tile_twice_safe()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.Alone);
        }

        [Fact]
        public void two_horizontal_adjacent()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(1, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.LeftNub);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.RightNub);
        }

        [Fact]
        public void three_horizontal_adjacent()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(1, 0));
            subject.PutTileAt(new Point(2, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.LeftNub);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.HorizontalNub);
            subject.GetClassAt(new Point(2, 0)).Should().Be(TileClass.RightNub);
        }

        [Fact]
        public void two_vertical_adjacent()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 1));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.TopNub);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.BottomNub);
        }

        [Fact]
        public void three_vertical_adjacent()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 1));
            subject.PutTileAt(new Point(0, 2));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.TopNub);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.VerticalNub);
            subject.GetClassAt(new Point(0, 2)).Should().Be(TileClass.BottomNub);
        }

        [Fact]
        public void two_by_two_square()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(1, 1));
            subject.PutTileAt(new Point(0, 1));
            subject.PutTileAt(new Point(1, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.TopLeftCorner);
            subject.GetClassAt(new Point(1, 1)).Should().Be(TileClass.BottomRightCorner);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.BottomLeftCorner);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.TopRightCorner);
        }

        [Fact]
        public void plus_sign_shape()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 1));
            subject.PutTileAt(new Point(1, 0));
            subject.PutTileAt(new Point(0, -1));
            subject.PutTileAt(new Point(-1, 0));

            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.CenterNub);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.BottomNub);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.RightNub);
            subject.GetClassAt(new Point(0, -1)).Should().Be(TileClass.TopNub);
            subject.GetClassAt(new Point(-1, 0)).Should().Be(TileClass.LeftNub);
        }

        [Fact]
        public void three_by_three_square()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(1, 1));
            subject.PutTileAt(new Point(1, -1));
            subject.PutTileAt(new Point(-1, -1));
            subject.PutTileAt(new Point(-1, 1));
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 1));
            subject.PutTileAt(new Point(1, 0));
            subject.PutTileAt(new Point(0, -1));
            subject.PutTileAt(new Point(-1, 0));

            subject.GetClassAt(new Point(1, 1)).Should().Be(TileClass.BottomRightCorner);
            subject.GetClassAt(new Point(1, -1)).Should().Be(TileClass.TopRightCorner);
            subject.GetClassAt(new Point(-1, -1)).Should().Be(TileClass.TopLeftCorner);
            subject.GetClassAt(new Point(-1, 1)).Should().Be(TileClass.BottomLeftCorner);
            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.BottomEdge);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.RightEdge);
            subject.GetClassAt(new Point(0, -1)).Should().Be(TileClass.TopEdge);
            subject.GetClassAt(new Point(-1, 0)).Should().Be(TileClass.LeftEdge);
        }

        [Fact]
        public void three_by_three_square_with_nubs()
        {
            var subject = new AutoTile();
            subject.PutTileAt(new Point(1, 1));
            subject.PutTileAt(new Point(1, -1));
            subject.PutTileAt(new Point(-1, -1));
            subject.PutTileAt(new Point(-1, 1));
            subject.PutTileAt(new Point(0, 0));
            subject.PutTileAt(new Point(0, 1));
            subject.PutTileAt(new Point(1, 0));
            subject.PutTileAt(new Point(0, -1));
            subject.PutTileAt(new Point(-1, 0));
            subject.PutTileAt(new Point(2, 0));
            subject.PutTileAt(new Point(-2, 0));
            subject.PutTileAt(new Point(0, 2));
            subject.PutTileAt(new Point(0, -2));

            subject.GetClassAt(new Point(1, 1)).Should().Be(TileClass.BottomRightCorner);
            subject.GetClassAt(new Point(1, -1)).Should().Be(TileClass.TopRightCorner);
            subject.GetClassAt(new Point(-1, -1)).Should().Be(TileClass.TopLeftCorner);
            subject.GetClassAt(new Point(-1, 1)).Should().Be(TileClass.BottomLeftCorner);
            subject.GetClassAt(new Point(0, 0)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(0, 1)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(1, 0)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(0, -1)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(-1, 0)).Should().Be(TileClass.CenterFill);
            subject.GetClassAt(new Point(0, 2)).Should().Be(TileClass.BottomNub);
            subject.GetClassAt(new Point(0, -2)).Should().Be(TileClass.TopNub);
            subject.GetClassAt(new Point(2, 0)).Should().Be(TileClass.RightNub);
            subject.GetClassAt(new Point(-2, 0)).Should().Be(TileClass.LeftNub);
        }
    }
}
