using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public struct Tile
    {

    }

    public class Level
    {
        public event Action ContentChanged;
        private readonly Dictionary<Point, Tile> tileMap = new Dictionary<Point, Tile>();

        public void PutTileAt(Point position, Tile tile)
        {
            this.tileMap[position] = tile;
            ContentChanged?.Invoke();
        }

        public Tuple<Point, Point> CalculateCorners()
        {
            var minTile = Point.Zero;
            var maxTile = Point.Zero;
            foreach (var tilePosition in tileMap.Keys)
            {
                minTile = new Point(Math.Min(minTile.X, tilePosition.X), Math.Min(minTile.Y, tilePosition.Y));
                maxTile = new Point(Math.Max(minTile.X, tilePosition.X), Math.Max(minTile.Y, tilePosition.Y));
            }

            return new Tuple<Point, Point>(minTile, maxTile);
        }
    }
}
