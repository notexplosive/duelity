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
        private readonly TileFrame[] brambleTiles;
        public bool ShowGrid { get; set; } = false;

        public TileGridRenderer(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            this.grid = RequireComponent<Grid>();
            this.tilesheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("tiles-sheet");
            this.brambleTiles = new TileFrame[]
            {
                TileFrame.Bramble0,
                TileFrame.Bramble1,
            };
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
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Wall, this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                    case TileImageTag.TileImage.Hook:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Hook, this.grid.TileToLocalPosition(location, true), floorDepth, Color.White); // hooks are floorDepth
                        break;
                    case TileImageTag.TileImage.Water:
                        this.tilesheet.DrawFrame(spriteBatch, (int)AutoTileClassToWaterFrame(this.grid.GetWaterClassAt(location)), this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                    case TileImageTag.TileImage.Ravine:
                        this.tilesheet.DrawFrame(spriteBatch, (int)AutoTileClassToRavineFrame(this.grid.GetRavineClassAt(location)), this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                    case TileImageTag.TileImage.Bramble:
                        this.tilesheet.DrawFrame(spriteBatch, GetRandomBrambleTile(location), this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                    case TileImageTag.TileImage.Bridge:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Bridge, this.grid.TileToLocalPosition(location, true), tileDepth, Color.White);
                        break;
                }
            }
            else
            {
                // floor
                DrawFloorTile(spriteBatch, location, floorDepth);
            }
        }

        private void DrawFloorTile(SpriteBatch spriteBatch, Point location, Depth floorDepth)
        {
            this.tilesheet.DrawFrame(spriteBatch, GetRandomFloorTile(location), this.grid.TileToLocalPosition(location, true),
                floorDepth + 10, Color.White);
        }

        private int GetRandomFloorTile(Point location)
        {
            var noise = NoiseAt(location);
            return (int)this.groundTiles[noise % this.groundTiles.Length];
        }
        
        private int GetRandomBrambleTile(Point location)
        {
            var noise = NoiseAt(location);
            return (int)this.brambleTiles[noise % this.brambleTiles.Length];
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
                    return TileFrame.WaterTopRight;
                case TileClass.TopLeftCorner:
                    return TileFrame.WaterTopLeft;
                case TileClass.BottomLeftCorner:
                    return TileFrame.WaterBottomLeft;
                case TileClass.BottomRightCorner:
                    return TileFrame.WaterBottomRight;
            }

            return TileFrame.WaterCenter;
        }
        
        private TileFrame AutoTileClassToRavineFrame(TileClass tileClass)
        {
            switch (tileClass)
            {
                case TileClass.Alone:
                    return TileFrame.RavineAloneNub;
                case TileClass.VerticalNub:
                    return TileFrame.RavineVerticalNub;
                case TileClass.HorizontalNub:
                    return TileFrame.RavineHorizontal;
                case TileClass.CenterNub:
                    return TileFrame.RavineCenterNub;
                case TileClass.BottomNub:
                    return TileFrame.RavineBottomNub;
                case TileClass.LeftNub:
                    return TileFrame.RavineLeftNub;
                case TileClass.RightNub:
                    return TileFrame.RavineRightNub;
                case TileClass.TopNub:
                    return TileFrame.RavineTopNub;
                case TileClass.CenterFill:
                    return TileFrame.RavineCenter;
                case TileClass.BottomEdge:
                    return TileFrame.RavineBottomEdge;
                case TileClass.LeftEdge:
                    return TileFrame.RavineLeftEdge;
                case TileClass.RightEdge:
                    return TileFrame.RavineRightEdge;
                case TileClass.TopEdge:
                    return TileFrame.RavineTopEdge;
                case TileClass.TopRightCorner:
                    return TileFrame.RavineTopRight;
                case TileClass.TopLeftCorner:
                    return TileFrame.RavineTopLeft;
                case TileClass.BottomLeftCorner:
                    return TileFrame.RavineBottomLeft;
                case TileClass.BottomRightCorner:
                    return TileFrame.RavineBottomRight;
            }

            return TileFrame.RavineCenter;
        }

        public void EnableGrid()
        {
            ShowGrid = true;
        }
    }
}