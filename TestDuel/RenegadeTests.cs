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

        public RenegadeTests()
        {
            Sokoban.Headless = true;
            var scene = new Scene(null);
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
    }
}
