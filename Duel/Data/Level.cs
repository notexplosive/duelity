using Microsoft.Xna.Framework;
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

        public void AddEntity(Entity entity)
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

        public Tuple<Point, Point> CalculateCorners()
        {
            var minTile = Point.Zero;
            var maxTile = Point.Zero;
            foreach (var tilePosition in tileMap.Keys)
            {
                minTile = new Point(Math.Min(minTile.X, tilePosition.X), Math.Min(minTile.Y, tilePosition.Y));
                maxTile = new Point(Math.Max(maxTile.X, tilePosition.X), Math.Max(maxTile.Y, tilePosition.Y));
            }

            return new Tuple<Point, Point>(minTile, maxTile);
        }

        public Entity[] AllEntities()
        {
            return this.entities.ToArray();
        }
    }
}
