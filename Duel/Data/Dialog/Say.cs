namespace Duel.Data.Dialog
{
    public class Say : IDialogEvent
    {
        public Speaker Speaker { get; }
        public string Text { get; }

        public Say(Speaker speaker, string text)
        {
            Speaker = speaker;
            Text = text;
        }
    }
}