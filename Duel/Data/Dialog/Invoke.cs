namespace Duel.Data.Dialog
{
    public class Invoke : IDialogEvent
    {
        public string EventName { get; }

        public Invoke(string eventName)
        {
            EventName = eventName;
        }
    }
}