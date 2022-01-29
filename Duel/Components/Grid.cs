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
        public Corners LevelCorners { get; private set; }
        private AutoTile WaterAutoTile { get; set; }
        private AutoTile RavineAutoTile { get; set; }


        public Grid(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            Recompute();

            this.level.TilemapChanged += Recompute;
        }

        private void Recompute()
        {
            LevelCorners = this.level.CalculateCorners();
            WaterAutoTile = this.level.ComputeAutoTile(TileImageTag.TileImage.Water, TileImageTag.TileImage.Bridge);
            RavineAutoTile = this.level.ComputeAutoTile(TileImageTag.TileImage.Ravine, TileImageTag.TileImage.BridgeOverRavine);
        }

        public Vector2 TileToLocalPosition(Point location, bool centered = true)
        {
            return location.ToVector2() * TileSize + (centered ? new Vector2(TileSize) / 2 : Vector2.Zero);
        }

        public TileClass GetWaterClassAt(Point location)
        {
            return WaterAutoTile.GetClassAt(location);
        }

        public TileClass GetRavineClassAt(Point location)
        {
            return RavineAutoTile.GetClassAt(location);
        }
    }
}
