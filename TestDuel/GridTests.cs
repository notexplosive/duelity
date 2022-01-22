using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class GridTests
    {
        private readonly Scene scene;
        private readonly Level level;
        private readonly Actor baseActor;
        private readonly Grid subject;

        public GridTests()
        {
            this.scene = new Scene(null);
            this.level = new Level();
            this.baseActor = this.scene.AddActor("ActorRoot");
            this.subject = new Grid(this.baseActor, this.level);
            new ActorRoot(this.baseActor, this.level);
        }

        private void FlushScene()
        {
            this.scene.FlushBuffers();
        }

        [Fact]
        public void actor_gets_placed_at_proper_position()
        {
            var entity = this.level.CreateEntityWithTags(new Point(5, 5));
            FlushScene();

            this.subject.transform.ChildAt(0).transform.LocalPosition.Should().Be(new Vector2(352, 352));
        }

        [Fact]
        public void actor_moves_when_entity_moves()
        {
            var entity = this.level.CreateEntityWithTags(new Point(5, 5));
            FlushScene();

            entity.WarpToPosition(new Point(5, 6));

            this.subject.transform.ChildAt(0).transform.LocalPosition.Should().Be(new Vector2(352, 416));
        }
    }
}
