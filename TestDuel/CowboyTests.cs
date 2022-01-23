using Duel.Components;
using Duel.Data;
using FluentAssertions;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Xunit;

namespace TestDuel
{
    public class CowboyTests
    {
        private readonly Entity playerEntity;
        private readonly CowboyMovement chargeComponent;
        private readonly Level level;
        private readonly Scene scene;
        private readonly Sokoban game;

        public CowboyTests()
        {
            Sokoban.Headless = true;
            this.scene = new Scene(null);
            this.game = new Sokoban(this.scene);

            this.playerEntity = this.game.CurrentLevel.PutEntityAt(new Point(0, 0), new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy)));
            this.chargeComponent = this.game.FindActor(this.playerEntity).GetComponent<CowboyMovement>();
            this.level = this.game.CurrentLevel;

            this.level.PutTileAt(new Point(0, 0), new TileTemplate());
            this.level.PutTileAt(new Point(10, 10), new TileTemplate());
        }

        private void DoChargeDown()
        {
            var charge = this.chargeComponent.CreateCharge(Direction.Down);
            foreach (var item in new CoroutineWrapper(this.chargeComponent.ChargeCoroutine(charge)))
            {
                // Automatically run through the entire coroutine
            }
        }

        [Fact]
        public void charge_to_edge_of_level()
        {
            var subject = new Charge(new Point(5, 5), Direction.Down, new LevelSolidProvider(this.level));

            subject.Path.Should().Contain(new ChargeHit(new Point(5, 9), Direction.Down));
        }

        [Fact]
        public void charge_cleaves_through_breakables()
        {
            var glass = new EntityTemplate(new Hittable(Hittable.Type.DestroyOnHit));

            var actor1 = this.game.FindActor(this.level.PutEntityAt(new Point(0, 6), glass));
            var actor2 = this.game.FindActor(this.level.PutEntityAt(new Point(0, 7), glass));
            var actor3 = this.game.FindActor(this.level.PutEntityAt(new Point(0, 8), glass));

            DoChargeDown();
            scene.FlushBuffers();

            actor1.IsDestroyed.Should().BeTrue();
            actor2.IsDestroyed.Should().BeTrue();
            actor3.IsDestroyed.Should().BeTrue();
        }

        [Fact]
        public void charge_into_heavy_metal_box()
        {
            var metalBox = new EntityTemplate(new Hittable(Hittable.Type.PushOnHit), new SolidTag(SolidTag.Type.Static));

            var boxInstance = this.level.PutEntityAt(new Point(0, 5), metalBox);

            DoChargeDown();

            this.playerEntity.Position.Should().Be(new Point(0, 4));
            boxInstance.Position.Should().Be(new Point(0, 6));
        }

        [Fact]
        public void charge_into_wall()
        {
            this.level.PutTileAt(new Point(0, 5), new TileTemplate(new SolidTag(SolidTag.Type.Static)));

            DoChargeDown();

            this.playerEntity.Position.Should().Be(new Point(0, 4));
        }
    }
}
