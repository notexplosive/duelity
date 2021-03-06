using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
            var game = new Sokoban(this.scene);

            this.playerEntity = game.CurrentLevel.PutEntityAt(new Point(0, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade)));
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
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().Contain(new Point(0, 9)).And.HaveCount(1);
        }

        [Fact]
        public void shot_through_entities_and_tiles()
        {
            // None of these traits block projectiles
            this.level.PutTileAt(new Point(0, 3), new TileTemplate(new Solid()));
            this.level.PutTileAt(new Point(0, 4), new TileTemplate(new Solid().PushOnBump()));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
        }

        [Fact]
        public void shot_blocked_by_tile()
        {
            this.level.PutTileAt(new Point(0, 3), new TileTemplate(new BlockProjectileTag()));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().Contain(new Point(0, 3)).And.HaveCount(1);
        }

        [Fact]
        public void shot_through_several_breakables()
        {
            var glass = new EntityTemplate(new DestroyOnHit(), new Solid().PushOnBump());
            this.level.PutEntityAt(new Point(0, 3), glass);
            this.level.PutEntityAt(new Point(0, 4), glass);
            this.level.PutEntityAt(new Point(0, 5), glass);

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().ContainInOrder(new Point(0, 3), new Point(0, 4), new Point(0, 5));
            bullet.LastHitLocation.Should().Be(new Point(0, 9));
        }

        [Fact]
        public void shot_blocked_by_entity()
        {
            this.level.PutEntityAt(new Point(0, 4), new EntityTemplate(new BlockProjectileTag()));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 4));
        }

        [Fact]
        public void shot_blocked_at_point_blank()
        {
            this.level.PutEntityAt(new Point(0, 1), new EntityTemplate(new BlockProjectileTag()));

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 1));
        }

        [Fact]
        public void shot_causes_push()
        {
            var pushable = this.level.PutEntityAt(new Point(0, 3), new EntityTemplate(new BlockProjectileTag(), new Solid().PushOnHit()));

            this.gunComponent.Shoot();

            pushable.Position.Should().Be(new Point(0, 4));
        }

        [Fact]
        public void shot_pushes_item_out_of_bounds()
        {
            // weird edge case I came across while testing
            var destroyedEntities = new List<Entity>();
            this.level.EntityDestroyRequested += (entity, type) =>
            {
                destroyedEntities.Add(entity);
                this.level.RemoveEntity(entity);
            };
            var pushedOnHit = this.level.PutEntityAt(new Point(0, 3), new EntityTemplate(new Solid().PushOnHit()));
            var pushable = this.level.PutEntityAt(new Point(0, 4), new EntityTemplate(new DestroyOnHit()));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 4));
            pushable.Position.Should().Be(new Point(0, 4));

            destroyedEntities.Should().Contain(pushable);

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 5));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 6));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 7));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 8));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 9));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 9));

            this.gunComponent.Shoot();

            pushedOnHit.Position.Should().Be(new Point(0, 9));
        }

        [Fact]
        public void shot_blocked_by_closed_door()
        {
            var closedDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, false));
            this.level.PutEntityAt(new Point(0, 5), closedDoor);

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 5));
        }

        [Fact]
        public void shot_not_blocked_by_opened_door()
        {
            var closedDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, false));
            this.level.ForceSignalOn(SignalColor.Red);
            this.level.PutEntityAt(new Point(0, 5), closedDoor);

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 9));
        }

        [Fact]
        public void shot_not_blocked_by_pre_open_door()
        {
            var openedDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, true));
            this.level.PutEntityAt(new Point(0, 5), openedDoor);

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 9));
        }

        [Fact]
        public void shot_blocked_by_newly_closed_door()
        {
            var openedDoor = new EntityTemplate(new SignalDoor(SignalColor.Red, true));
            this.level.ForceSignalOn(SignalColor.Red);
            this.level.PutEntityAt(new Point(0, 5), openedDoor);

            var bullet = this.gunComponent.CreateBullet();

            bullet.StartPosition.Should().Be(new Point(0, 0));
            bullet.HitAtLeastOneThing.Should().BeTrue();
            bullet.WasBlocked.Should().BeTrue();
            bullet.HitLocations.Should().HaveCount(1).And.Contain(new Point(0, 5));
        }
    }
}
