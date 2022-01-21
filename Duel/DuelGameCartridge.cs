using Duel.Components;
using Duel.Data;
using Machina.Engine;
using Machina.Engine.Assets;
using Machina.Engine.Cartridges;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel
{
    public class DuelGameCartridge : GameCartridge
    {
        public DuelGameCartridge() : base(new Point(1600, 900), ResizeBehavior.KeepAspectRatio)
        {
        }

        public override void OnGameLoad(GameSpecification specification, MachinaRuntime runtime)
        {
            SceneLayers.BackgroundColor = Color.Black;

            var level = new Level();

            level.PutTileAt(new Point(-10, -10), new Tile());
            level.PutTileAt(new Point(10, 10), new Tile());

            var gameScene = SceneLayers.AddNewScene();
            var levelActor = gameScene.AddActor("Level");
            levelActor.transform.Depth -= 200;

            new LevelRenderer(levelActor, level);
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {

        }
    }
}
