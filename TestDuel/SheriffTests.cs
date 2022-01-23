using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class SheriffTests
    {
        private readonly Entity playerEntity;
        private readonly UseLasso lassoComponent;
        private readonly Level level;

        public SheriffTests()
        {
            Sokoban.Headless = true;
            var scene = new Scene(null);
            var game = new Sokoban(scene);

            this.playerEntity = game.CurrentLevel.PutEntityAt(new Point(0, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff)));
            this.lassoComponent = game.FindActor(this.playerEntity).GetComponent<UseLasso>();
            this.level = game.CurrentLevel;
        }

        private LassoProjectile ThrowLassoDown()
        {
            var projectile = this.lassoComponent.CreateProjectile();
            foreach (var item in new CoroutineWrapper(this.lassoComponent.LassoCoroutine(projectile)))
            {
                // Automatically run through the entire coroutine
            }

            return projectile;
        }

        [Fact]
        public void lasso_returns_when_missed()
        {
            var lasso = ThrowLassoDown();

            lasso.Valid.Should().BeTrue();
            lasso.FoundHook.Should().BeFalse();
            lasso.FoundPullableEntity.Should().BeFalse();

            lasso.LassoLandingPosition.Should().Be(new Point(0, 3)); // max range of lasso
            this.playerEntity.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void lasso_with_wall_right_in_front_of_you_is_invalid()
        {
            var wall = new TileTemplate();
            wall.Tags.AddTag(new BlockProjectileTag());
            this.level.PutTileAt(new Point(0, 1), wall); // right in front of the player

            var lasso = ThrowLassoDown();

            lasso.Valid.Should().BeFalse();
            lasso.FoundHook.Should().BeFalse();
            lasso.FoundPullableEntity.Should().BeFalse();
            lasso.LassoLandingPosition.Should().Be(new Point(0, 0));
            this.playerEntity.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void lasso_is_blocked_when_blocked()
        {
            var wall = new TileTemplate();
            wall.Tags.AddTag(new BlockProjectileTag());
            this.level.PutTileAt(new Point(0, 2), wall);

            var lasso = ThrowLassoDown();

            lasso.Valid.Should().BeTrue();
            lasso.FoundHook.Should().BeFalse();
            lasso.FoundPullableEntity.Should().BeFalse();
            lasso.LassoLandingPosition.Should().Be(new Point(0, 1));
            this.playerEntity.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void lasso_pulls_pullables()
        {
            var pullable = this.level.PutEntityAt(new Point(0, 2), new EntityTemplate(new Grapplable(Grapplable.Type.PulledByLasso)));

            var lasso = ThrowLassoDown();

            lasso.Valid.Should().BeTrue();
            lasso.FoundHook.Should().BeTrue();
            lasso.FoundPullableEntity.Should().BeTrue();
            lasso.LassoLandingPosition.Should().Be(new Point(0, 2));
            pullable.Position.Should().Be(new Point(0, 1));
            this.playerEntity.Position.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void lasso_jumps_caster_to_hook()
        {
            var hook = new TileTemplate();
            hook.Tags.AddTag(new Grapplable(Grapplable.Type.Static));
            this.level.PutTileAt(new Point(0, 2), hook);

            var lasso = ThrowLassoDown();

            lasso.Valid.Should().BeTrue();
            lasso.FoundHook.Should().BeTrue();
            lasso.FoundPullableEntity.Should().BeFalse();
            lasso.LassoLandingPosition.Should().Be(new Point(0, 2));
            this.playerEntity.Position.Should().Be(new Point(0, 2));
        }
    }
}
