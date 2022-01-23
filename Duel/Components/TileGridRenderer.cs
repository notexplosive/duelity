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
    public class TileGridRenderer : BaseComponent
    {
        private readonly Level level;
        private readonly Grid grid;

        public TileGridRenderer(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            this.grid = RequireComponent<Grid>();
        }

        private Corners LevelCorners => this.grid.LevelCorners;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var entireSpaceRectangle = new RectangleF(this.grid.TileToLocalPosition(this.LevelCorners.TopLeft, false), this.grid.TileToLocalPosition(this.LevelCorners.BottomRight - this.LevelCorners.TopLeft, false));
            spriteBatch.FillRectangle(entireSpaceRectangle, Color.Tan, transform.Depth + 200);
            for (int x = this.LevelCorners.TopLeft.X; x < this.LevelCorners.BottomRight.X; x++)
            {
                for (int y = this.LevelCorners.TopLeft.Y; y < this.LevelCorners.BottomRight.Y; y++)
                {
                    DrawCell(spriteBatch, new Point(x, y), this.level.GetTileAt(new Point(x, y)));
                }
            }
        }

        public void DrawCell(SpriteBatch spriteBatch, Point location, TileTemplate tile)
        {
            var tileDepth = transform.Depth + 100;
            var floorDepth = tileDepth + 50;

            if ((location.X % 2 == 0 && location.Y % 2 == 1) || (location.X % 2 == 1 && location.Y % 2 == 0))
            {
                var rect = new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize));
                rect.Inflate(-5, -5);
                spriteBatch.DrawRectangle(rect, new Color(Color.Tan.R - 20, Color.Tan.G - 20, Color.Tan.B - 20), 1f, floorDepth);
            }

            if (tile.Tags.TryGetTag(out TileImageTag imageTag))
            {
                switch (imageTag.Image)
                {
                    case TileImageTag.TileImage.Floor:
                        // draw nothing
                        break;
                    case TileImageTag.TileImage.Wall:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.Orange, tileDepth);
                        break;
                    case TileImageTag.TileImage.Hook:
                        spriteBatch.DrawCircle(new CircleF(this.grid.TileToLocalPosition(location, true), 10), 10, Color.LightBlue, 2f, tileDepth);
                        break;
                    case TileImageTag.TileImage.Water:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.LightBlue, tileDepth);
                        break;
                    case TileImageTag.TileImage.Ravine:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.Black, tileDepth);
                        break;
                    case TileImageTag.TileImage.Bramble:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.SlateGray, tileDepth);
                        break;
                    case TileImageTag.TileImage.FilledWater:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.LightBlue, tileDepth);
                        var fillRect = new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize));
                        fillRect.Inflate(-10, -10);
                        spriteBatch.FillRectangle(fillRect, Color.Brown, tileDepth);
                        break;
                    case TileImageTag.TileImage.Bridge:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.LightBlue, tileDepth - 1);
                        var bridgeRect = new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize));
                        bridgeRect.Inflate(-5, -5);
                        spriteBatch.FillRectangle(bridgeRect, Color.Orange, tileDepth - 1);
                        break;
                }
            }
        }
    }
}
