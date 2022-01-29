using Xunit;
using Duel.Data;
using Duel.Data.Dialog;
using FluentAssertions;
using System.Collections.Generic;

namespace TestDuel.Dialog
{
    public class ScreenplayTest
    {
        [Fact]
        public void happy_path()
        {
            Screenplay screenplay = new Screenplay("Content/test_dialog.tsv");


            screenplay.GetConversation("test_conversation").Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new Say(Speaker.SheriffNormal, "...."),
                new Invoke("invoke_string"),
                new Say(Speaker.SheriffSpooked, ".........!!")
            }));

            screenplay.GetConversation("test_conversation_2").Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new Invoke("invoke_string_2")
            }));
        }
    }
}