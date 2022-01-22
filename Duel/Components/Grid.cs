using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class Grid : BaseComponent
    {
        public static readonly int TileSize = 64;

        private readonly Level level;
        private Corners levelCorners;

        public Grid(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            RecomputeCorners();

            this.level.TilemapChanged += RecomputeCorners;
        }

        private void RecomputeCorners()
        {
            this.levelCorners = this.level.CalculateCorners();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = this.levelCorners.TopLeft.X; x < this.levelCorners.BottomRight.X; x++)
            {
                for (int y = this.levelCorners.TopLeft.Y; y < this.levelCorners.BottomRight.Y; y++)
                {
                    DrawCell(spriteBatch, new Point(x, y), this.level.GetTileAt(new Point(x, y)));
                }
            }
        }

        public void DrawCell(SpriteBatch spriteBatch, Point location, TileTemplate tile)
        {
            var color = Color.White;
            var tileDepth = transform.Depth + 100;


            if (tile.Tags.HasTag<SolidTag>())
            {
                spriteBatch.FillRectangle(new RectangleF(TileToLocalPosition(location, false), new Point(TileSize)), Color.Orange, tileDepth - 1);
            }

            spriteBatch.DrawRectangle(new RectangleF(TileToLocalPosition(location, false), new Point(TileSize)), color, 1f, tileDepth);
        }

        public Vector2 TileToLocalPosition(Point location, bool centered = true)
        {
            return location.ToVector2() * TileSize + (centered ? new Vector2(TileSize) / 2 : Vector2.Zero);
        }
    }
}
