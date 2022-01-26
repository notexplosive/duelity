using Duel.Data;
using FluentAssertions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Xunit;

namespace TestDuel
{
    public class KeyDoorTests
    {
        private readonly Level level;
        private readonly EntityTemplate door;
        private readonly EntityTemplate key;

        public KeyDoorTests()
        {
            this.level = new Level(new Corners(new Point(-10, 10), new Point(10, 10)));
            this.door = new EntityTemplate(new KeyDoor(SignalColor.Blue), new Solid());
            this.key = new EntityTemplate(new Key(SignalColor.Blue));
        }

        [Fact]
        public void key_destroys_door_when_bumped()
        {
            var key = this.level.PutEntityAt(Point.Zero, this.key);
            var door = this.level.PutEntityAt(new Point(1, 0), this.door);

            var destroyedEntites = new List<Entity>();

            this.level.EntityDestroyRequested += (entity, type) =>
            {
                destroyedEntites.Add(entity);
            };

            key.WalkAndPushInDirection(Direction.Right);

            destroyedEntites.Should().Contain(key).And.Contain(door).And.HaveCount(2);
        }
    }
}
