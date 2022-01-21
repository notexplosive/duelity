using Duel.Data;
using FluentAssertions;
using Xunit;

namespace TestDuel
{
    public class EntityTests
    {
        [Fact]
        public void entities_are_not_same_unless_share_id()
        {
            var subject1 = new Entity();
            var subject2 = new Entity();

            var subject1Copy = subject1;
            var subject2Copy = subject2;

            subject1.Should().NotBe(subject2);
            subject1.Should().Be(subject1Copy);
            subject2.Should().Be(subject2Copy);
        }
    }
}
