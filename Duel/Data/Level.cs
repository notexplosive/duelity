﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public delegate void EntityEvent(Entity entity);

    public class Level
    {
        public event Action TilemapChanged;
        public event EntityEvent EntityAdded;
        private readonly List<Entity> entities = new List<Entity>();

        private readonly Dictionary<Point, TileTemplate> tileMap = new Dictionary<Point, TileTemplate>();

        public Level(Corners corners)
        {
            PutTileAt(corners.TopLeft, new TileTemplate());
            PutTileAt(corners.BottomRight, new TileTemplate());
        }

        public Level() : this(new Corners(Point.Zero, Point.Zero))
        {

        }

        public Entity CreateEntity(Point startingPosition, params Tag[] tags)
        {
            var entity = new Entity(new LevelSolidProvider(this));

            foreach (var tag in tags)
            {
                entity.Tags.AddTag(tag);
            }

            entity.WarpToPosition(startingPosition);

            AddEntity(entity);

            return entity;
        }

        private void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
            EntityAdded?.Invoke(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            this.entities.Remove(entity);
        }

        public void PutTileAt(Point position, TileTemplate tile)
        {
            this.tileMap[position] = tile;
            TilemapChanged?.Invoke();
        }

        public Corners CalculateCorners()
        {
            var minTile = Point.Zero;
            var maxTile = Point.Zero;
            foreach (var tilePosition in tileMap.Keys)
            {
                minTile = new Point(Math.Min(minTile.X, tilePosition.X), Math.Min(minTile.Y, tilePosition.Y));
                maxTile = new Point(Math.Max(maxTile.X, tilePosition.X), Math.Max(maxTile.Y, tilePosition.Y));
            }

            return new Corners(minTile, maxTile);
        }

        public TileTemplate GetTileAt(Point position)
        {
            this.tileMap.TryGetValue(position, out TileTemplate result);
            if (result == null)
            {
                return new TileTemplate();
            }

            return result;
        }

        public Entity[] AllEntities()
        {
            return this.entities.ToArray();
        }

        public bool IsOutOfBounds(Point position)
        {
            var corners = CalculateCorners();
            return position.X < corners.TopLeft.X || position.Y < corners.TopLeft.Y || position.X > corners.BottomRight.X || position.Y > corners.BottomRight.Y;
        }

        public IEnumerable<Entity> AllEntitiesAt(Point position)
        {
            foreach (var entity in this.entities)
            {
                if (entity.Position == position)
                {
                    yield return entity;
                }
            }
        }
    }
}
