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
        public DuelGameCartridge(Point renderResolution /*arg so it can be exposed to the editor*/) : base(renderResolution, ResizeBehavior.KeepAspectRatio)
        {
        }

        public override void OnGameLoad(GameSpecification specification, MachinaRuntime runtime)
        {
            SceneLayers.BackgroundColor = Color.Black;

            var gameScene = SceneLayers.AddNewScene();
            var game = new Sokoban(gameScene);

            // player characters
            var sheriff = new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff));
            var renegade = new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade));
            var cowboy = new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy));
            var knight = new EntityTemplate(new PlayerTag(PlayerTag.Type.Knight));

            var templates = TemplateLibrary.Build();

            game.CurrentLevel.PutEntityAt(new Point(3, 3), sheriff);

            game.CurrentLevel.PutPropAt(new Vector2(200, 200), templates.GetPropTemplate("large_cactus"));

            PostLoad(game);
        }

        protected virtual void PostLoad(Sokoban game)
        {
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {
            foreach (var levelName in Sokoban.LevelNames)
            {
                loader.AddMachinaAssetCallback(levelName, () => LevelData.LoadLevelDataFromDisk(levelName));
            }

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
            loader.AddMachinaAssetCallback("bennigan-hover", () => new LinearFrameAnimation(20, 2));
            loader.AddMachinaAssetCallback("bennigan-land", () => new LinearFrameAnimation(22, 1));
        }
    }
}