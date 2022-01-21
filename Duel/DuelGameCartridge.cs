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

            var game = SceneLayers.AddNewScene();
            var levelActor = game.AddActor("Level");

            new LevelRenderer(levelActor, new Level());
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {

        }
    }
}
