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
            game.CurrentLevel.PutTileAt(new Point(-15, -9), new TileTemplate());
            game.CurrentLevel.PutTileAt(new Point(15, 9), new TileTemplate());

            // Tiles
            var wall = new TileTemplate(new Solid(), new TileImageTag(TileImageTag.TileImage.Wall),
                new BlockProjectileTag());
            var water = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Water), new UnfilledWater());
            var bridge = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bridge), new Collapses(water));
            var ravine = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Ravine), new Ravine());
            var bramble = new TileTemplate(new TileImageTag(TileImageTag.TileImage.Bramble), new Solid());
            var hook = new TileTemplate(new Grapplable(Grapplable.Type.Static),
                new TileImageTag(TileImageTag.TileImage.Hook));

            // Entities
            var glass = new EntityTemplate(
                new DestroyOnHit(),
                new SimpleEntityImage(EntityFrameSet.GlassBottle),
                new Solid().PushOnBump(),
                new Grapplable(Grapplable.Type.PulledByLasso),
                new WaterFiller(WaterFiller.Type.Floats)
            );

            var anvil = new EntityTemplate(
                new Solid().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(EntityFrameSet.Anvil),
                new WaterFiller(WaterFiller.Type.Sinks)
            );

            var crate = new EntityTemplate(
                new Solid().PushOnBump(),
                new BlockProjectileTag(),
                new Grapplable(Grapplable.Type.PulledByLasso),
                new SimpleEntityImage(EntityFrameSet.Crate),
                new DestroyOnHit(),
                new WaterFiller(WaterFiller.Type.Floats)
            );

            var barrel = new EntityTemplate(
                new Solid().PushOnBump().PushOnHit(),
                new BlockProjectileTag(),
                new SimpleEntityImage(EntityFrameSet.Barrel),
                new WaterFiller(WaterFiller.Type.Floats)
            );

            var miasma = new EntityTemplate(
                new BlockProjectileTag(),
                new MiasmaImageTag()
            );

            var redLever = new EntityTemplate(
                new BlockProjectileTag(),
                new Solid(),
                new ToggleSignal(SignalColor.Red).OnBump().OnGrapple().OnHit(),
                new LeverImageTag(SignalColor.Red)
            );

            var blueLever = new EntityTemplate( // duplicate of red, refactor this when we make editor
                new BlockProjectileTag(),
                new Solid(),
                new ToggleSignal(SignalColor.Blue).OnBump().OnGrapple().OnHit(),
                new LeverImageTag(SignalColor.Blue)
            );

            var redPressurePlate = new EntityTemplate(
                new EnableSignalWhenSteppedOn(SignalColor.Red),
                new PressurePlateImageTag(SignalColor.Red)
            );

            var bluePressurePlate = new EntityTemplate( // duplicate
                new EnableSignalWhenSteppedOn(SignalColor.Blue),
                new PressurePlateImageTag(SignalColor.Blue)
            );

            var openRedDoor = new EntityTemplate(
                new SignalDoor(SignalColor.Red, true)
            );

            var closedRedDoor = new EntityTemplate( // duplicate
                new SignalDoor(SignalColor.Red, false)
            );

            var openBlueDoor = new EntityTemplate( // duplicate
                new SignalDoor(SignalColor.Blue, true)
            );

            var blueKey = new EntityTemplate(
                new Solid().PushOnBump(),
                new Key(SignalColor.Blue),
                new Grapplable(Grapplable.Type.PulledByLasso)
            );

            var blueKeyDoor = new EntityTemplate(
                new Solid(),
                new BlockProjectileTag(),
                new KeyDoor(SignalColor.Blue)
            );

            // player characters
            var sheriff = new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff));
            var renegade = new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade));
            var cowboy = new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy));
            var knight = new EntityTemplate(new PlayerTag(PlayerTag.Type.Knight));

            game.CurrentLevel.PutTileAt(new Point(4, 5), ravine);
            game.CurrentLevel.PutTileAt(new Point(4, 6), ravine);
            game.CurrentLevel.PutTileAt(new Point(4, 7), water);
            game.CurrentLevel.PutTileAt(new Point(4, 8), water);

            game.CurrentLevel.PutEntityAt(new Point(0, 0), openRedDoor);
            game.CurrentLevel.PutEntityAt(new Point(0, 1), closedRedDoor);
            game.CurrentLevel.PutEntityAt(new Point(0, 2), redLever);

            game.CurrentLevel.PutEntityAt(new Point(4, 1), anvil);
            game.CurrentLevel.PutEntityAt(new Point(4, 2), barrel);
            game.CurrentLevel.PutEntityAt(new Point(4, 3), glass);
            game.CurrentLevel.PutEntityAt(new Point(4, 4), crate);

            game.CurrentLevel.PutEntityAt(new Point(5, 2), blueKey);
            game.CurrentLevel.PutEntityAt(new Point(7, 2), blueKeyDoor);

            game.CurrentLevel.PutEntityAt(new Point(3, 3), renegade);
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