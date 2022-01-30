using Duel.Components;
using Duel.Data;
using Duel.Data.Dialog;
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
        private readonly List<Chapter> chapters = new List<Chapter>();
        public Screenplay Screenplay { get; }
        private int chapterIndex = 0;

        public static DuelGameCartridge Instance { get; private set; } // ew i stepped in a singleton

        public DuelGameCartridge(Point renderResolution) : base(renderResolution, ResizeBehavior.KeepAspectRatio)
        {
            Instance = this;

            Screenplay = new Screenplay(MachinaClient.FileSystem.ReadTextAppDataThenLocal("screenplay.tsv").Result);

            // Chapter sequence for the whole game
            var emptyConvo = new Conversation(new List<IDialogEvent>());

            this.chapters = new List<Chapter>();

            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Sheriff, ZoneTileset.Thistown, Screenplay.GetConversation("sheriff_intro_1A")));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Renegade, ZoneTileset.Thistown, Screenplay.GetConversation("renegade_intro_1A")));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Cowboy, ZoneTileset.Thistown, Screenplay.GetConversation("cowboy_intro_1A"))); 
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Knight, ZoneTileset.Thistown, Screenplay.GetConversation("knight_intro_1A"))); 
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Sheriff, ZoneTileset.Thistown, emptyConvo));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Renegade, ZoneTileset.Thistown, emptyConvo));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Cowboy, ZoneTileset.Thistown, emptyConvo));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Knight, ZoneTileset.Thistown, emptyConvo));

            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Sheriff, ZoneTileset.Oasis, emptyConvo));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Renegade, ZoneTileset.Oasis, emptyConvo));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Cowboy, ZoneTileset.Oasis, emptyConvo));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Knight, ZoneTileset.Oasis, emptyConvo));

            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Sheriff, ZoneTileset.Mines, emptyConvo));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Renegade, ZoneTileset.Mines, emptyConvo));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Cowboy, ZoneTileset.Mines, emptyConvo));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Knight, ZoneTileset.Mines, emptyConvo));

            // this.chapters.Add(new EndCinematicChapter());
        }

        public override void OnGameLoad(GameSpecification specification, MachinaRuntime runtime)
        {
            SceneLayers.BackgroundColor = Color.Black;
            LoadGameOrEditor();
        }

        protected virtual void LoadGameOrEditor()
        {
            var gameScene = SceneLayers.AddNewScene();
            GetCurrentChapterAndIncrement().Load(gameScene);
        }

        public Chapter GetCurrentChapterAndIncrement()
        {
            return this.chapters[this.chapterIndex++];
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {
            foreach (var levelName in Sokoban.LevelContentNames)
            {
                loader.AddMachinaAssetCallback(levelName, () => LevelData.LoadLevelDataFromDisk(levelName));
            }

            loader.AddMachinaAssetCallback("characters-sheet",
                () => new GridBasedSpriteSheet("characters", new Point(64)));
            loader.AddMachinaAssetCallback("tiles-sheet", () => new GridBasedSpriteSheet("tiles", new Point(64)));
            loader.AddMachinaAssetCallback("entities-sheet", () => new GridBasedSpriteSheet("entities", new Point(64)));
            loader.AddMachinaAssetCallback("npcs-sheet", () => new GridBasedSpriteSheet("npcs", new Point(64))); // replace with NPCs sheet once we have it
            loader.AddMachinaAssetCallback("portraits", () => new GridBasedSpriteSheet("portraits", new Point(128)));

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