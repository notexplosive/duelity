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
        private readonly List<IChapter> chapters = new List<IChapter>();
        public Screenplay Screenplay { get; }
        private int chapterIndex = 0;

        public static DuelGameCartridge Instance { get; private set; } // ew i stepped in a singleton
        public MusicPlayer MusicPlayer { get; private set; }

        public DuelGameCartridge(Point renderResolution) : base(renderResolution, ResizeBehavior.KeepAspectRatio)
        {
            MachinaRuntime.SkipIntro = true;
            Instance = this;

            Screenplay = new Screenplay(MachinaClient.FileSystem.ReadTextAppDataThenLocal("screenplay.tsv").Result);

            // Chapter sequence for the whole game
            var emptyConvo = new Conversation(new List<IDialogEvent>());

            this.chapters = new List<IChapter>();

            this.chapters.Add(new TitleScreen());
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Sheriff, ZoneTileset.Thistown, TrackName.ThistownA, Screenplay.GetConversation("sheriff_intro_1A")));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Renegade, ZoneTileset.Thistown, TrackName.ThistownA, Screenplay.GetConversation("renegade_intro_1A")));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Cowboy, ZoneTileset.Thistown, TrackName.ThistownA, Screenplay.GetConversation("cowboy_intro_1A")));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Knight, ZoneTileset.Thistown, TrackName.ThistownA, Screenplay.GetConversation("knight_intro_1A")));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Sheriff, ZoneTileset.Thistown, TrackName.ThistownB, Screenplay.GetConversation("sheriff_intro_1B")));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Renegade, ZoneTileset.Thistown, TrackName.ThistownB, emptyConvo));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Cowboy, ZoneTileset.Thistown, TrackName.ThistownB, Screenplay.GetConversation("steven_intro_1B")));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Knight, ZoneTileset.Thistown, TrackName.ThistownB, emptyConvo));

            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Sheriff, ZoneTileset.Oasis, TrackName.Oasis, Screenplay.GetConversation("sheriff_intro_oasis")));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Renegade, ZoneTileset.Oasis, TrackName.Oasis, emptyConvo));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Cowboy, ZoneTileset.Oasis, TrackName.Oasis, emptyConvo));
            this.chapters.Add(new Chapter("level_3", PlayerTag.Type.Knight, ZoneTileset.Oasis, TrackName.Oasis, emptyConvo));

            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Sheriff, ZoneTileset.Mines, TrackName.Mines, Screenplay.GetConversation("sheriff_intro_mine")));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Renegade, ZoneTileset.Mines, TrackName.Mines, emptyConvo));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Cowboy, ZoneTileset.Mines, TrackName.Mines, emptyConvo));
            this.chapters.Add(new Chapter("level_4", PlayerTag.Type.Knight, ZoneTileset.Mines, TrackName.Mines, emptyConvo));
            this.chapters.Add(new Finale());
        }

        public override void OnGameLoad(GameSpecification specification, MachinaRuntime runtime)
        {
            MusicPlayer = new MusicPlayer();
            SceneLayers.BackgroundColor = Color.Black;
            LoadGameOrEditor();
        }

        protected virtual void LoadGameOrEditor()
        {
            // GAME ONLY -- Editor overrides this function
            var gameScene = SceneLayers.AddNewScene();
            GetCurrentChapterAndIncrement().Load(gameScene);
        }

        public IChapter GetCurrentChapterAndIncrement()
        {
            return this.chapters[this.chapterIndex++];
        }

        public IChapter PeekNextChapter()
        {
            return this.chapters[this.chapterIndex];
        }

        public IChapter GetCurrentChapter()
        {
            try
            {
                var chapter = this.chapters[this.chapterIndex - 1];

                if (chapter == null)
                {
                    return this.chapters[1];
                }
                return chapter;
            }
            catch (Exception)
            {
                return this.chapters[1];
            }
        }

        public override void PrepareDynamicAssets(AssetLoader loader, MachinaRuntime runtime)
        {
            foreach (var levelName in Sokoban.LevelContentNames)
            {
                loader.AddMachinaAssetCallback(levelName, () => LevelData.LoadLevelDataFromDisk(levelName));
            }

            loader.AddMachinaAssetCallback("characters-sheet",
                () => new GridBasedSpriteSheet("characters", new Point(64)));
            loader.AddMachinaAssetCallback("silos-sheet", () => new GridBasedSpriteSheet("silos", new Point(250)));
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