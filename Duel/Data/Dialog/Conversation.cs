using System.Collections.Generic;

namespace Duel.Data.Dialog
{
    public class Conversation
    {
        public List<IDialogEvent> Events { get; }

        public Conversation(List<IDialogEvent> events)
        {
            Events = events;
        }
    }
}