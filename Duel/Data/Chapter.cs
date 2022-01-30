using Duel.Data.Dialog;
using Machina.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public enum ZoneTileset
    {
        Thistown,
        Oasis,
        Mines
    }

    public class Chapter
    {
        private readonly ZoneTileset zoneTileset;

        public Chapter(string levelName, PlayerTag.Type player, ZoneTileset zoneTileset, TrackName levelTrack, Conversation conversationOnStartup)
        {
            LevelName = levelName;
            Player = player;
            ConversationOnStartup = conversationOnStartup;
            LevelTrack = levelTrack;
            this.zoneTileset = zoneTileset;
        }

        public string LevelName { get; }
        public PlayerTag.Type Player { get; }
        public Conversation ConversationOnStartup { get; }
        public TrackName LevelTrack { get; }

        public void Load(Scene scene)
        {
            var game = new Sokoban(scene, this.zoneTileset);
            game.PlayLevel(MachinaClient.Assets.GetMachinaAsset<LevelData>(LevelName), Player);
            game.StartDialogue(ConversationOnStartup);


            if (Player == PlayerTag.Type.Knight)
            {
                DuelGameCartridge.Instance.MusicPlayer.PlayTrack(TrackName.Knight);
            }
            else
            {
                DuelGameCartridge.Instance.MusicPlayer.PlayTrack(LevelTrack);
            }
        }
    }
}
