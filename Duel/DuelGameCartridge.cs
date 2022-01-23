using Duel.Components;
using Duel.Data;
using Machina.Data;
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
        public DuelGameCartridge() : base(new Point(960, 540), ResizeBehavior.KeepAspectRatio)
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


            var glass = new EntityTemplate(new Hittable(Hittable.Type.DestroyOnHit));
            game.CurrentLevel.PutEntityAt(new Point(0, 0), glass);
            game.CurrentLevel.PutEntityAt(new Point(0, 2), glass);
            game.CurrentLevel.PutEntityAt(new Point(0, 3), glass);
            game.CurrentLevel.PutEntityAt(new Point(0, 4), glass);
            game.CurrentLevel.PutEntityAt(new Point(0, 5), glass);


            game.CurrentLevel.PutEntityAt(new Point(3, 2), new EntityTemplate(new Hittable(Hittable.Type.PushOnHit), new BlockProjectileTag()));
            game.CurrentLevel.PutEntityAt(new Point(4, 2), new EntityTemplate(new Hittable(Hittable.Type.DestroyOnHit), new SolidTag(SolidTag.Type.Pushable)));

            game.CurrentLevel.PutEntityAt(new Point(3, 3), new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy)));
            game.CurrentLevel.PutEntityAt(new Point(6, 5), new EntityTemplate(new SolidTag(SolidTag.Type.Pushable), new Grapplable(Grapplable.Type.PulledByLasso)));
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {
            loader.AddMachinaAssetCallback("characters-sheet", () => new GridBasedSpriteSheet("characters", new Point(64)));
            loader.AddMachinaAssetCallback("ernesto-idle", () => new LinearFrameAnimation(0, 2));
            loader.AddMachinaAssetCallback("ernesto-move", () => new LinearFrameAnimation(2, 1));

            loader.AddMachinaAssetCallback("miranda-idle", () => new LinearFrameAnimation(6, 2));
            loader.AddMachinaAssetCallback("miranda-move", () => new LinearFrameAnimation(8, 1));

            loader.AddMachinaAssetCallback("steven-idle", () => new LinearFrameAnimation(12, 2));
            loader.AddMachinaAssetCallback("steven-move", () => new LinearFrameAnimation(14, 2));
        }
    }
}
