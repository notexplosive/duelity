namespace Duel.Data.Dialog
{
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
}