using System.Collections.Generic;

namespace Duel.Data.Dialog
{
    public class Conversation
    {
        public List<IDialogEvent> Events { get; }

        public Conversation(List<IDialogEvent> events) {
            Events = events;
        }
    }

    public interface IDialogEvent {}

    public class Say : IDialogEvent
    {
        public Speaker Speaker { get; }
        public string Text { get; }

        public Say(Speaker speaker, string text) {
            Speaker = speaker;
            Text = text;
        }
    }

    public class Move : IDialogEvent
    {
        public Direction Direction { get; }
        public Character Character { get; }

        public Move(Direction direction, Character character)
        {
            Direction = direction;
            Character = character;
        }
    }

    public class PressZ : IDialogEvent
    {
        public Character Character { get; }

        public PressZ(Character character)
        {
            Character = character;
        }
    }
}