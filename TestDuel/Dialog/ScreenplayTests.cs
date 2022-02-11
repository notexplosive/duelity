using Xunit;
using Duel.Data;
using Duel.Data.Dialog;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace TestDuel.Dialog
{
    public class ScreenplayTest
    {
        [Fact]
        public void happy_path()
        {
            var text = File.OpenText("Content/test_dialog.tsv");
            Screenplay screenplay = new Screenplay(text.ReadToEnd());


            screenplay.GetConversation("test_conversation").Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new Say(Speaker.SheriffNormal, "....", Color.White),
                new Invoke("invoke_string"),
                new Say(Speaker.SheriffSpooked, ".........!!", Color.White)
            }));

            screenplay.GetConversation("test_conversation_2").Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new Invoke("invoke_string_2")
            }));
        }
    }
}