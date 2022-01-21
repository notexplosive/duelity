﻿using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using System;
using Xunit;

namespace TestDuel
{
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
            this.subject = new ActorRoot(this.baseActor, this.level);
        }

        private void FlushScene()
        {
            this.scene.FlushBuffers();
        }

        [Fact]
        public void actor_root_creates_actor_from_level()
        {

            this.level.AddEntity(new Entity());
            this.level.AddEntity(new Entity());

            this.subject.transform.ChildCount.Should().Be(2);
        }

        [Fact]
        public void actor_root_deletes_actors_when_destroyed()
        {
            var firstEntity = new Entity();
            this.level.AddEntity(firstEntity);
            this.level.AddEntity(new Entity());
            this.subject.transform.ChildAt(1).Destroy();
            FlushScene();

            this.subject.transform.ChildCount.Should().Be(1);
            this.level.AllEntities().Should().HaveCount(1).And.Contain(firstEntity);
        }
    }
}
