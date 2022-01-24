using Duel.Data;
using FluentAssertions;
using Xunit;

namespace TestDuel
{
    public class SignalTests
    {
        [Fact]
        public void off_by_default()
        {
            var subject = new SignalState();

            subject.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void turn_off_while_off()
        {
            var subject = new SignalState();

            subject.TurnOff(SignalColor.Red);

            subject.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void turn_on()
        {
            var subject = new SignalState();

            subject.TurnOn(SignalColor.Red);

            subject.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void turn_on_twice()
        {
            var subject = new SignalState();

            subject.TurnOn(SignalColor.Red);
            subject.TurnOn(SignalColor.Red);


            subject.IsOn(SignalColor.Red).Should().BeTrue();
        }

        [Fact]
        public void turn_on_twice_then_off()
        {
            var subject = new SignalState();

            subject.TurnOn(SignalColor.Red);
            subject.TurnOn(SignalColor.Red);
            subject.TurnOff(SignalColor.Red);

            subject.IsOn(SignalColor.Red).Should().BeFalse();
        }

        [Fact]
        public void toggle()
        {
            var subject = new SignalState();

            subject.Toggle(SignalColor.Red);
            subject.IsOn(SignalColor.Red).Should().BeTrue();
            subject.Toggle(SignalColor.Red);
            subject.IsOn(SignalColor.Red).Should().BeFalse();
            subject.Toggle(SignalColor.Red);
            subject.IsOn(SignalColor.Red).Should().BeTrue();
        }
    }
}
