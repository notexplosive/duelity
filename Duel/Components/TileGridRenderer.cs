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
        private readonly ActorRoot actorRoot;
        private readonly Level level;
        private readonly Grid grid;
        private readonly SpriteSheet entitiesSheet;
        private readonly SpriteSheet tilesheet;
        private readonly TileFrame[] groundTiles;
        private readonly TileFrame[] brambleTiles;

        private float timer = 0;
        public bool ShowGrid { get; set; } = false;

        public TileGridRenderer(Actor actor, Level level) : base(actor)
        {
            this.actorRoot = RequireComponent<ActorRoot>();
            this.level = level;
            this.grid = RequireComponent<Grid>();
            this.entitiesSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet");
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
                TileFrame.Floor4
            };

        }

        private Corners LevelCorners => this.grid.LevelCorners;

        public bool GhostMode { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = this.LevelCorners.TopLeft.X; x <= this.LevelCorners.BottomRight.X; x++)
            {
                for (int y = this.LevelCorners.TopLeft.Y; y <= this.LevelCorners.BottomRight.Y; y++)
                {
                    DrawCell(spriteBatch, new Point(x, y), this.level.GetTileAt(new Point(x, y)));
                }
            }
        }

        public override void Update(float dt)
        {
            this.timer += dt;
        }

        public void DrawCell(SpriteBatch spriteBatch, Point location, TileTemplate tile)
        {
            var tileDepth = transform.Depth + 100;
            var floorDepth = tileDepth + 50;

            var color = new Color(Color.White, GhostMode ? 0.5f : 1f);
            var realPos = transform.Position + this.grid.TileToLocalPosition(location, true);

            if (tile.Tags.TryGetTag(out TileImageTag imageTag))
            {
                switch (imageTag.Image)
                {
                    case TileImageTag.TileImage.Wall:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Wall, realPos, tileDepth, color);
                        DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
                        break;
                    case TileImageTag.TileImage.Hook:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Hook, realPos, floorDepth - new Depth(10), color);
                        DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
                        break;
                    case TileImageTag.TileImage.Water:
                        this.tilesheet.DrawFrame(spriteBatch, (int)AutoTileClassToWaterFrame(this.grid.GetWaterClassAt(location)), realPos, tileDepth, color);
                        DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
                        break;
                    case TileImageTag.TileImage.Ravine:
                        this.tilesheet.DrawFrame(spriteBatch, (int)AutoTileClassToRavineFrame(this.grid.GetRavineClassAt(location)), realPos, tileDepth, color);
                        DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
                        break;
                    case TileImageTag.TileImage.Bramble:
                        this.tilesheet.DrawFrame(spriteBatch, GetRandomBrambleTile(location), realPos, tileDepth, color);
                        DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
                        break;
                    case TileImageTag.TileImage.Bridge:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.Bridge, realPos, tileDepth, color);
                        break;
                    case TileImageTag.TileImage.BridgeOverRavine:
                        this.tilesheet.DrawFrame(spriteBatch, (int)TileFrame.BridgeOverRavine, realPos, tileDepth, color);
                        break;
                }
            }
            else
            {
                // floor
                DrawFloorTile(spriteBatch, color, realPos, location, floorDepth);
            }

            if (tile.Tags.TryGetTag(out FilledWater water))
            {
                if (water.FillingEntity.Tags.TryGetTag(out SimpleEntityImage simpleEntityImage) && this.actorRoot.IsActorDestroyed(water.FillingEntity))
                {
                    var angle = MathF.Sin(this.timer + new NoiseBasedRNG((uint)NoiseAt(location)).NextFloat() * MathF.PI * 2) / 16;
                    this.entitiesSheet.DrawFrame(spriteBatch, simpleEntityImage.EntityFrameSet.Normal, realPos, 0.8f, angle, XYBool.False, tileDepth - 1, color);
                }
            }
        }

        private void DrawFloorTile(SpriteBatch spriteBatch, Color color, Vector2 realPos, Point location, Depth floorDepth)
        {
            this.tilesheet.DrawFrame(spriteBatch, GetRandomFloorTile(location), realPos,
                floorDepth + 10, color);
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