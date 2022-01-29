namespace Duel.Data.Dialog
{
    public class PressZ : IDialogEvent
    {
        public Character Character { get; }

        public PressZ(Character character)
        {
            Character = character;
        }
    }
}