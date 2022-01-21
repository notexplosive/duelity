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
            var busySignal = new BusySignal();

            busySignal.IsBusy().Should().BeFalse();
        }
    }
}
