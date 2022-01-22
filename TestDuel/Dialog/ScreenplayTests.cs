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
            Screenplay screenplay = new Screenplay("../../../Content/test_dialog.tsv");


            screenplay.Conversations["test_conversation"].Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new Say(Speaker.SheriffNormal, "...."),
                new Move(Direction.Left, Character.Sheriff),
                new Move(Direction.Down, Character.Sheriff),
                new Move(Direction.Down, Character.Sheriff),
                new Say(Speaker.SheriffSpooked, ".........!!")
            }));

            screenplay.Conversations["test_conversation_2"].Should().BeEquivalentTo(new Conversation(new List<IDialogEvent> {
                new PressZ(Character.Renegade)
            }));
        }
    }
}