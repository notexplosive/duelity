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

            // When screenplay is ready, replace with commented out constructor and remove the `new Conversation` lines below
            Screenplay = new Screenplay(); // new Screenplay(MachinaClient.FileSystem.ReadTextAppDataThenLocal("screenplay.tsv").Result);

            // Remove all of this when screenplay is ready
            var sarAndSheriff = new Conversation(new List<IDialogEvent>
                {
                    new Say(Speaker.Sar, "Thistown ain't big enough for the both of us..."),
                    new Say(Speaker.Sar, "...on account of the city limits being about 8 square feet."),
                    new Say(Speaker.SheriffNormal, "..."),
                    new Say(Speaker.Sar, "The name's Sar, what can I do ya for?"),
                    new Say(Speaker.SheriffNormal, "I'm looking for..."),
                    new Say(Speaker.SheriffDeepBow, ".... ..... ......"),
                    new Say(Speaker.Sar, "Oh! Him. Last I heard he was in Tarnation."),
                    new Say(Speaker.SheriffSpooked, "What?! In Tarnation?"),
                    new Say(Speaker.Sar, "Afraid so. I know it's a long ways away but there's a trail up north that'll put you on the right track."),
                    new Say(Speaker.SheriffNormal, "..."),
                    new Say(Speaker.SheriffDeepBow, "..."),
                    new Say(Speaker.SheriffNormal, "Thanks."),
                });
            Screenplay.AddConversation("sar_sheriff", sarAndSheriff);

            // once I have CSVs these can be totally data-driven, but for now here's some inline dialogue for testing

            var level1LaunchConvo = new Conversation(new List<IDialogEvent>
                {
                    new Say(Speaker.SheriffNormal, "......"),

                    new Say(Speaker.SheriffNormal, "Check out my hat..."),
                    new Say(Speaker.SheriffDeepBow, "...do you like it?"),
                    new Say(Speaker.SheriffNormal, "Pretty cool huh?"),
                    new Say(Speaker.SheriffNormal, "...."),
                    new Say(Speaker.SheriffSpooked, "You don't like it?!"),
                });

            var level2LaunchConvo = new Conversation(new List<IDialogEvent>
                {
                    new Say(Speaker.SheriffNormal, "Tarnation's gonna be a pretty long walk. Guess I better get walking."),
                    new Say(Speaker.SheriffNormal, "Oh I just remembered! I can press R to restart levels."),
                    new Say(Speaker.SheriffSpooked, "Unless there are any heavy barrels or anvils that I can't grapple onto."),
                });




            // Chapter sequence for the whole game
            this.chapters = new List<Chapter>();
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Sheriff, level1LaunchConvo));
            this.chapters.Add(new Chapter("level_2", PlayerTag.Type.Sheriff, level2LaunchConvo));
            this.chapters.Add(new Chapter("level_1", PlayerTag.Type.Renegade, /*should be something else*/level2LaunchConvo));
            // todo: the rest of them
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
            loader.AddMachinaAssetCallback("npcs-sheet", () => new GridBasedSpriteSheet("portraits", new Point(64))); // replace with NPCs sheet once we have it
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