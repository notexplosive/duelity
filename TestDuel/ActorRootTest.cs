using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
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

    public class ActorRootTest
    {
        private readonly Scene scene;
        private readonly Level level;
        private readonly Actor baseActor;
        private readonly ActorRoot subject;

        public ActorRootTest()
        {
            this.scene = new Scene(null);
            this.level = new Level();
            this.baseActor = this.scene.AddActor("ActorRoot");
            new Grid(this.baseActor, this.level);
            this.subject = new ActorRoot(this.baseActor, this.level);
        }

        private void FlushScene()
        {
            this.scene.FlushBuffers();
        }

        [Fact]
        public void actor_root_creates_actor_from_level()
        {

            this.level.PutEntityAt(Point.Zero, new EntityTemplate());
            this.level.PutEntityAt(Point.Zero, new EntityTemplate());

            this.subject.transform.ChildCount.Should().Be(2);
        }

        [Fact]
        public void actor_root_deletes_actors_when_destroyed()
        {
            var firstEntity = this.level.PutEntityAt(Point.Zero, new EntityTemplate());
            this.level.PutEntityAt(Point.Zero, new EntityTemplate());
            this.subject.transform.ChildAt(1).Destroy();
            FlushScene();

            this.subject.transform.ChildCount.Should().Be(1);
            this.level.AllEntities().Should().HaveCount(1).And.Contain(firstEntity);
        }
    }
}
