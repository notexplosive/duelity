using System;
using System.IO;
using System.Collections.Generic;

namespace Duel.Data.Dialog
{
    public class Screenplay
    {
        public Dictionary<string, Conversation> Conversations { get; }

        public Screenplay(string screenplayFilePath)
        {
            Conversations = new Dictionary<string, Conversation>();
            StreamReader reader = File.OpenText(screenplayFilePath);
            string currentConversationName = "";
            List<IDialogEvent> currentConversationEventList = null;
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();
                if (line != "") {
                    string[] parts = line.Split("\t");
                    string type = parts[0];
                    if (type == "conversation") {
                        if (currentConversationEventList != null) {
                            Conversations.Add(currentConversationName, new Conversation(currentConversationEventList));
                        }
                        currentConversationName = parts[1];
                        currentConversationEventList = new List<IDialogEvent>();
                    } else if (type == "say") {
                        currentConversationEventList.Add(new Say(Speaker.getSpeaker(parts[1]), parts[2]));
                    } else if (type == "invoke") {
                        currentConversationEventList.Add(new Invoke(parts[1]));
                    }
                }
            }
            if (currentConversationEventList != null) {
                Conversations.Add(currentConversationName, new Conversation(currentConversationEventList));
            }
        }
    }
}