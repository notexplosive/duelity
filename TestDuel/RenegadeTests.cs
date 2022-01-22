using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class RenegadeTests
    {
        private readonly Entity playerEntity;
        private readonly UseGun gunComponent;
        private readonly Level level;
        private readonly Scene scene;

        public RenegadeTests()
        {
            Sokoban.Headless = true;
            this.scene = new Scene(null);
            var game = new Sokoban(scene);

            this.playerEntity = game.CurrentLevel.CreateEntity(new Point(0, 0), new PlayerTag(PlayerTag.Type.Renegade));
            this.gunComponent = game.FindActor(this.playerEntity).GetComponent<UseGun>();
            this.level = game.CurrentLevel;

            this.level.PutTileAt(new Point(0, 0), new TileTemplate());
            this.level.PutTileAt(new Point(10, 10), new TileTemplate());
        }

        [Fact]
        public void shot_out_of_bounds()
        {
            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeFalse();
            bullet.WasBlocked.Should().BeFalse();
            bullet.HitLocations.Should().Contain(new Point(0, 11)).And.HaveCount(1);
        }

        [Fact]
        public void shot_through_entities_and_tiles()
        {
            // None of these traits block projectiles
            this.level.PutTileAt(new Point(0, 3), new TileTemplate(new SolidTag(SolidTag.Type.Static)));
            this.level.PutTileAt(new Point(0, 4), new TileTemplate(new SolidTag(SolidTag.Type.Pushable)));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeFalse();
            bullet.WasBlocked.Should().BeFalse();
        }

        [Fact]
        public void shot_blocked_by_tile()
        {
            this.level.PutTileAt(new Point(0, 3), new TileTemplate(new BlockProjectileTag()));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeFalse();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().Contain(new Point(0, 3)).And.HaveCount(1);
        }

        [Fact]
        public void shot_through_several_breakables()
        {
            this.level.CreateEntity(new Point(0, 3), new Hittable(Hittable.Type.DestroyOnHit));
            this.level.CreateEntity(new Point(0, 4), new Hittable(Hittable.Type.DestroyOnHit));
            this.level.CreateEntity(new Point(0, 5), new Hittable(Hittable.Type.DestroyOnHit));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeFalse();
            bullet.HitLocations.Should().ContainInOrder(new Point(0, 3), new Point(0, 4), new Point(0, 5));
        }

        [Fact]
        public void shot_blocked_by_entity()
        {
            this.level.CreateEntity(new Point(0, 4), new BlockProjectileTag());

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeFalse();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 4));
        }

        [Fact]
        public void shot_blocked_at_point_blank()
        {
            this.level.CreateEntity(new Point(0, 1), new BlockProjectileTag());

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeFalse();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 1));
        }

        [Fact]
        public void shot_causes_push()
        {
            var pushable = this.level.CreateEntity(new Point(0, 3), new BlockProjectileTag(), new Hittable(Hittable.Type.PushOnHit));

            this.gunComponent.Shoot();

            pushable.Position.Should().Be(new Point(0, 4));
        }

        [Fact]
        public void shot_pushes_item_out_of_bounds()
        {
            // weird edge case I came across while testing
            var pushedOnHit = this.level.CreateEntity(new Point(0, 3), new Hittable(Hittable.Type.PushOnHit));
            var pushable = this.level.CreateEntity(new Point(0, 4), new Hittable(Hittable.Type.DestroyOnHit), new SolidTag(SolidTag.Type.Pushable));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 4));
            pushable.Position.Should().Be(new Point(0, 5));

            this.scene.FlushBuffers(); // because an actor got destroyed. ugh.

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 5));
            pushable.Position.Should().Be(new Point(0, 5)); // didn't move because it got destroyed (should really have a better way to measure that)

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 6));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 7));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 8));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 9));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 10));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 10));
        }
    }
}
