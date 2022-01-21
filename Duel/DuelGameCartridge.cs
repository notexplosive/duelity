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

            var gameScene = SceneLayers.AddNewScene();
            var game = new Sokoban(gameScene);

            game.CurrentLevel.PutTileAt(new Point(-10, -10), new Tile());
            game.CurrentLevel.PutTileAt(new Point(10, 10), new Tile());

            var player = new Entity();
            player.Tags.AddTag(new PlayerTag(PlayerTag.Type.Sheriff));
            player.WarpToPosition(new Point(3, 3));

            game.CurrentLevel.AddEntity(player);
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {

        }
    }
}
