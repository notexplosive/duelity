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

            // Tiles
            var wall = new TileTemplate(new Solid(), new TileImageTag(TileImageTag.TileImage.Wall),
                new BlockProjectileTag());
            var water = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water), new Solid());
            var bridge = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bridge), new Collapses(water));
            var ravine = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Ravine));
            var bramble = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bramble), new Solid());
            var hook = new TileTemplate(new Grapplable(Grapplable.Type.Static),
                new TileImageTag(TileImageTag.TileImage.Hook));

            // Entities
            var glass = new EntityTemplate(
                new DestroyOnHit(),
                new SimpleEntityImage(SimpleEntityImage.EntityFrameSet.GlassBottle),
                new Solid().PushOnBump(),
                new Grapplable(Grapplable.Type.PulledByLasso)
            );

            var anvil = new EntityTemplate(
                new Solid().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(SimpleEntityImage.EntityFrameSet.Anvil)
            );

            var crate = new EntityTemplate(
                new Solid().PushOnBump(),
                new BlockProjectileTag(),
                new Grapplable(Grapplable.Type.PulledByLasso),
                new SimpleEntityImage(SimpleEntityImage.EntityFrameSet.Crate),
                new DestroyOnHit()
            );

            var barrel = new EntityTemplate(
                new Solid().PushOnBump().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(SimpleEntityImage.EntityFrameSet.Barrel)
            );

            var miasma = new EntityTemplate(
                new BlockProjectileTag(),
                new MiasmaImageTag()
            );

            var lever = new EntityTemplate(
                new BlockProjectileTag(),
                new Solid(),
                new ToggleSignal(SignalColor.Red).OnBump().OnGrapple().OnHit(),
                new LeverImageTag(SignalColor.Red)
            );

            // player characters
            var sheriff = new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff));
            var renegade = new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade));
            var cowboy = new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy));
            var knight = new EntityTemplate(new PlayerTag(PlayerTag.Type.Knight));

            game.CurrentLevel.PutTileAt(new Point(5, 6), wall);
            game.CurrentLevel.PutTileAt(new Point(5, 5), wall);
            game.CurrentLevel.PutTileAt(new Point(5, 4), wall);
            game.CurrentLevel.PutTileAt(new Point(0, 6), wall);
            game.CurrentLevel.PutTileAt(new Point(2, 2), water);
            game.CurrentLevel.PutTileAt(new Point(3, 2), water);
            game.CurrentLevel.PutTileAt(new Point(2, 3), water);
            game.CurrentLevel.PutTileAt(new Point(3, 3), bridge);
            game.CurrentLevel.PutTileAt(new Point(6, 6), hook);

            game.CurrentLevel.PutEntityAt(new Point(0, 0), glass);
            game.CurrentLevel.PutEntityAt(new Point(0, 1), miasma);
            game.CurrentLevel.PutEntityAt(new Point(0, 2), lever);
            game.CurrentLevel.PutEntityAt(new Point(3, 5), anvil);
            game.CurrentLevel.PutEntityAt(new Point(3, 6), crate);
            game.CurrentLevel.PutEntityAt(new Point(3, 2), barrel);
            game.CurrentLevel.PutEntityAt(new Point(4, 2), glass);

            game.CurrentLevel.PutEntityAt(new Point(3, 3), cowboy);
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {
            loader.AddMachinaAssetCallback("characters-sheet",
                () => new GridBasedSpriteSheet("characters", new Point(64)));
            loader.AddMachinaAssetCallback("tiles-sheet", () => new GridBasedSpriteSheet("tiles", new Point(64)));
            loader.AddMachinaAssetCallback("entities-sheet", () => new GridBasedSpriteSheet("entities", new Point(64)));

            loader.AddMachinaAssetCallback("ernesto-idle", () => new LinearFrameAnimation(0, 2));
            loader.AddMachinaAssetCallback("ernesto-move", () => new LinearFrameAnimation(2, 1));
            loader.AddMachinaAssetCallback("ernesto-jump", () => new LinearFrameAnimation(3, 1));
            loader.AddMachinaAssetCallback("ernesto-throw", () => new LinearFrameAnimation(4, 1));

            loader.AddMachinaAssetCallback("miranda-idle", () => new LinearFrameAnimation(6, 2));
            loader.AddMachinaAssetCallback("miranda-move", () => new LinearFrameAnimation(8, 1));
            loader.AddMachinaAssetCallback("miranda-shoot", () => new LinearFrameAnimation(9, 1));

            loader.AddMachinaAssetCallback("steven-idle", () => new LinearFrameAnimation(12, 2));
            loader.AddMachinaAssetCallback("steven-move", () => new LinearFrameAnimation(14, 2));

            loader.AddMachinaAssetCallback("bennigan-idle", () => new LinearFrameAnimation(18, 2));
            loader.AddMachinaAssetCallback("bennigan-hover", () => new LinearFrameAnimation(20, 1));
            loader.AddMachinaAssetCallback("bennigan-land", () => new LinearFrameAnimation(21, 1));
            loader.AddMachinaAssetCallback("bennigan-swing", () => new LinearFrameAnimation(22, 1));
        }
    }
}