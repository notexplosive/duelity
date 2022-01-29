using Duel.Data.Dialog;
using Machina.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class Chapter
    {
        public Chapter(string levelName, PlayerTag.Type player, Conversation conversationOnStartup)
        {
            LevelName = levelName;
            Player = player;
            ConversationOnStartup = conversationOnStartup;
        }

        public string LevelName { get; }
        public PlayerTag.Type Player { get; }
        public Conversation ConversationOnStartup { get; }

        public void Load(Scene scene)
        {
            var game = new Sokoban(scene);
            game.PlayLevel(MachinaClient.Assets.GetMachinaAsset<LevelData>(LevelName), Player);
            game.StartDialogue(ConversationOnStartup);
        }
    }
}
