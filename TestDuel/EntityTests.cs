using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
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

        [Fact]
        public void entity_cannot_walk_into_solid()
        {
            var solidProvider = new FakeSolidProvider();
            solidProvider.BecomeSolidAt(new Point(1, 1));
            var subject = new Entity(solidProvider, "");
            subject.WarpToPosition(new Point(1, 2));
            subject.WalkAndPushInDirection(Direction.Up);

            subject.Position.Should().Be(new Point(1, 2));
        }
    }
}
