using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
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
        private readonly SpriteSheet tilesheet;
        private readonly TileFrame[] groundTiles;
        public bool ShowGrid { get; set; } = false;

        public TileGridRenderer(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            this.grid = RequireComponent<Grid>();
            this.tilesheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("tiles-sheet");
            this.groundTiles = new TileFrame[] {
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor0,
                TileFrame.Floor1,
                TileFrame.Floor2,
                TileFrame.Floor3,
                TileFrame.Floor4 };
        }

        private Corners LevelCorners => this.grid.LevelCorners;

        public override void Draw(SpriteBatch spriteBatch)
        {
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

            if (this.ShowGrid)
            {
                if ((location.X % 2 == 0 && location.Y % 2 == 1) || (location.X % 2 == 1 && location.Y % 2 == 0))
                {
                    var rect = new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize));
                    rect.Inflate(-5, -5);
                    spriteBatch.DrawRectangle(rect, new Color(Color.Tan.R - 10, Color.Tan.G - 10, Color.Tan.B - 10), 1f, floorDepth);
                }
            }

            if (tile.Tags.TryGetTag(out TileImageTag imageTag))
            {
                switch (imageTag.Image)
                {
                    case TileImageTag.TileImage.Wall:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.Orange, tileDepth);
                        break;
                    case TileImageTag.TileImage.Hook:
                        spriteBatch.DrawCircle(new CircleF(this.grid.TileToLocalPosition(location, true), 10), 10, Color.LightBlue, 2f, tileDepth);
                        break;
                    case TileImageTag.TileImage.Water:
                        this.tilesheet.DrawFrame(spriteBatch, (int)AutoTileClassToWaterFrame(this.grid.GetWaterClassAt(location)), this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                    case TileImageTag.TileImage.Ravine:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.Black, tileDepth);
                        break;
                    case TileImageTag.TileImage.Bramble:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.SlateGray, tileDepth);
                        break;
                    case TileImageTag.TileImage.Bridge:
                        spriteBatch.FillRectangle(new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize)), Color.LightBlue, tileDepth - 1);
                        var bridgeRect = new RectangleF(this.grid.TileToLocalPosition(location, false), new Point(Grid.TileSize));
                        bridgeRect.Inflate(-5, -5);
                        spriteBatch.FillRectangle(bridgeRect, Color.Orange, tileDepth - 1);
                        break;
                }
            }
            else
            {
                // floor
                this.tilesheet.DrawFrame(spriteBatch, GetRandomFloorTile(location), this.grid.TileToLocalPosition(location, true), floorDepth + 10, Color.White);
            }
        }

        private int GetRandomFloorTile(Point location)
        {
            var noise = NoiseAt(location);
            return (int)this.groundTiles[noise % this.groundTiles.Length];
        }

        public int NoiseAt(Point position)
        {
            return Math.Abs((int)Squirrel3.Noise(position.X | (position.Y << (sizeof(int) / 2)), 1523));
        }

        private TileFrame AutoTileClassToWaterFrame(TileClass tileClass)
        {
            switch (tileClass)
            {
                case TileClass.Alone:
                    return TileFrame.WaterAloneNub;
                case TileClass.VerticalNub:
                    return TileFrame.WaterVerticalNub;
                case TileClass.HorizontalNub:
                    return TileFrame.WaterHorizontal;
                case TileClass.CenterNub:
                    return TileFrame.WaterCenterNub;
                case TileClass.BottomNub:
                    return TileFrame.WaterBottomNub;
                case TileClass.LeftNub:
                    return TileFrame.WaterLeftNub;
                case TileClass.RightNub:
                    return TileFrame.WaterRightNub;
                case TileClass.TopNub:
                    return TileFrame.WaterTopNub;
                case TileClass.CenterFill:
                    return TileFrame.WaterCenter;
                case TileClass.BottomEdge:
                    return TileFrame.WaterBottomEdge;
                case TileClass.LeftEdge:
                    return TileFrame.WaterLeftEdge;
                case TileClass.RightEdge:
                    return TileFrame.WaterRightEdge;
                case TileClass.TopEdge:
                    return TileFrame.WaterTopEdge;
                case TileClass.TopRightCorner:
                    return TileFrame.WaterRightEdge;
                case TileClass.TopLeftCorner:
                    return TileFrame.WaterTopLeft;
                case TileClass.BottomLeftCorner:
                    return TileFrame.WaterBottomLeft;
                case TileClass.BottomRightCorner:
                    return TileFrame.WaterBottomRight;
            }

            return TileFrame.WaterCenter;
        }

        public void EnableGrid()
        {
            ShowGrid = true;
        }
    }
}
