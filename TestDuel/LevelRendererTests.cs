using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class LevelRendererTests
    {
        private readonly Scene scene;
        private readonly Level level;
        private readonly Actor baseActor;
        private readonly LevelRenderer subject;

        public LevelRendererTests()
        {
            this.scene = new Scene(null);
            this.level = new Level();
            this.baseActor = this.scene.AddActor("ActorRoot");
            new ActorRoot(this.baseActor, this.level);
            this.subject = new LevelRenderer(this.baseActor, this.level);
        }

        private void FlushScene()
        {
            this.scene.FlushBuffers();
        }

        [Fact]
        public void actor_gets_placed_at_proper_position()
        {
            var entity = new Entity();
            entity.WarpToPosition(new Point(5, 5));

            this.level.AddEntity(entity);
            FlushScene();

            this.subject.transform.ChildAt(0).transform.LocalPosition.Should().Be(new Vector2(352, 352));
        }
    }
}
