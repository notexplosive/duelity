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


            // Corners
            game.CurrentLevel.PutTileAt(new Point(1, 1), new TileTemplate());
            game.CurrentLevel.PutTileAt(new Point(10, 10), new TileTemplate());

            // Walls
            var wall = new TileTemplate();
            wall.Tags.AddTag(new SolidTag(SolidTag.Type.Static));
            wall.Tags.AddTag(new TileImageTag(TileImageTag.TileImage.Wall));
            wall.Tags.AddTag(new BlockProjectileTag());
            game.CurrentLevel.PutTileAt(new Point(5, 6), wall);
            game.CurrentLevel.PutTileAt(new Point(5, 5), wall);
            game.CurrentLevel.PutTileAt(new Point(5, 4), wall);

            var hook = new TileTemplate();
            hook.Tags.AddTag(new Grapplable(Grapplable.Type.Static));
            hook.Tags.AddTag(new TileImageTag(TileImageTag.TileImage.Hook));
            game.CurrentLevel.PutTileAt(new Point(6, 6), hook);

            game.CurrentLevel.CreateEntity(new Point(3, 3), new PlayerTag(PlayerTag.Type.Sheriff));
            game.CurrentLevel.CreateEntity(new Point(6, 5), new SolidTag(SolidTag.Type.Pushable), new Grapplable(Grapplable.Type.PulledByLasso));
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {

        }
    }
}
