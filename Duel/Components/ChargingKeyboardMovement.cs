using Duel.Data;
using Machina.Components;
using Machina.Engine;
using System;

namespace Duel.Components
{
    public class ChargingKeyboardMovement : BaseComponent
    {
        private readonly BufferedKeyboardListener keyboard;
        private readonly Entity entity;
        private readonly LevelSolidProvider solidProvider;

        public ChargingKeyboardMovement(Actor actor, Entity entity, LevelSolidProvider solidProvider) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;
            this.solidProvider = solidProvider;
            this.keyboard.LeftPressed += Move(Direction.Left);
            this.keyboard.RightPressed += Move(Direction.Right);
            this.keyboard.DownPressed += Move(Direction.Down);
            this.keyboard.UpPressed += Move(Direction.Up);
        }

        private Action Move(Direction direction)
        {
            return
                () =>
                {
                    var projectedMove = new Charge(entity.Position, direction, this.solidProvider);
                    this.entity.ChargeToPosition(projectedMove.LandingPosition);
                    foreach (var hit in projectedMove.RammedPositions)
                    {
                        this.solidProvider.ApplyHitAt(hit, direction);
                    }
                };
        }

        public void Charge(Direction direction)
        {
            Move(direction)(); // exec a Move (only useful for tests)
        }
    }
}
