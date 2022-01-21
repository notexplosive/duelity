using Duel.Data;
using FluentAssertions;
using Xunit;

namespace TestDuel
{
    public class BusySignalTests
    {
        [Fact]
        public void busy_signal_is_not_busy_by_default()
        {
            var subject = new BusySignal();

            subject.IsBusy().Should().BeFalse();
        }

        [Fact]
        public void busy_signal_is_busy_when_given_action()
        {
            var subject = new BusySignal();

            subject.Add(new BusyFunction("AlwaysBusy", () => true));

            subject.IsBusy().Should().BeTrue();
            foreach (var busyFunction in subject.PendingBusyFunctions())
            {
                busyFunction.Name.Should().Be("AlwaysBusy");
            }
        }
    }
}
