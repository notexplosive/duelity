using System;
using System.IO;
using System.Collections.Generic;

namespace Duel.Data.Dialog
{
    public class Screenplay
    {
        private readonly Dictionary<string, Conversation> conversations = new Dictionary<string, Conversation>();
        private readonly HashSet<string> usedKeys = new HashSet<string>();

        public Screenplay()
        {
            // empty constructor to make testing easier
        }

        public Screenplay(string screenplayFilePath)
        {
            StreamReader reader = File.OpenText(screenplayFilePath);
            string currentConversationName = "";
            List<IDialogEvent> currentConversationEventList = null;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line != "")
                {
                    string[] parts = line.Split("\t");
                    string type = parts[0];
                    if (type == "conversation")
                    {
                        if (currentConversationEventList != null)
                        {
                            AddConversation(currentConversationName, new Conversation(currentConversationEventList));
                        }
                        currentConversationName = parts[1];
                        currentConversationEventList = new List<IDialogEvent>();
                    }
                    else if (type == "say")
                    {
                        currentConversationEventList.Add(new Say(Speaker.getSpeaker(parts[1]), parts[2]));
                    }
                    else if (type == "invoke")
                    {
                        currentConversationEventList.Add(new Invoke(parts[1]));
                    }
                }
            }
            if (currentConversationEventList != null)
            {
                AddConversation(currentConversationName, new Conversation(currentConversationEventList));
            }
        }

        public Conversation GetConversation(string key)
        {
            this.usedKeys.Add(key);
            return this.conversations[key];
        }

        public bool HasConversationAlreadyHappened(string key)
        {
            return this.usedKeys.Contains(key);
        }

        public void AddConversation(string key, Conversation conversation)
        {
            this.conversations.Add(key, conversation);
        }
    }
}