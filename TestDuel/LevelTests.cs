using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System;
using Xunit;

namespace TestDuel
{
    public class LevelTests
    {
        [Fact]
        public void level_corners_default_at_zero()
        {
            var subject = new Level();

            subject.CalculateCorners().Should().Be(new Tuple<Point, Point>(Point.Zero, Point.Zero));
        }

        [Fact]
        public void level_corners_are_correct()
        {
            var subject = new Level();

            subject.PutTileAt(new Point(-3, 1), new TileTemplate());
            subject.PutTileAt(new Point(-4, 4), new TileTemplate());
            subject.PutTileAt(new Point(3, -6), new TileTemplate());

            subject.CalculateCorners().Should().Be(new Tuple<Point, Point>(new Point(-4, -6), new Point(3, 4)));
        }

        [Fact]
        public void create_entity_fires_event()
        {
            var subject = new Level();
            Entity createdEntity = null;

            subject.EntityAdded += (entity) =>
            {
                createdEntity = entity;
            };

            Entity realEntity = new Entity();
            subject.AddEntity(realEntity);

            createdEntity.Should().Be(realEntity);
        }

        [Fact]
        public void place_tile_fires_event()
        {
            var subject = new Level();
            var hitCount = 0;

            subject.TilemapChanged += () =>
            {
                hitCount++;
            };

            subject.PutTileAt(new Point(1, 1), new TileTemplate());
            subject.PutTileAt(new Point(1, 1), new TileTemplate());
            subject.PutTileAt(new Point(1, 1), new TileTemplate());

            hitCount.Should().Be(3);
        }
    }
}
