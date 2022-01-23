using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

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
                    this.solidProvider.ApplyHitAt(projectedMove.RammedPosition, direction);
                };
        }
    }

    public class NormalKeyboardMovement : BaseComponent
    {
        private readonly Entity entity;
        private readonly BufferedKeyboardListener keyboard;

        public NormalKeyboardMovement(Actor actor, Entity entity) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;

            this.keyboard.LeftPressed += Move(Direction.Left);
            this.keyboard.RightPressed += Move(Direction.Right);
            this.keyboard.DownPressed += Move(Direction.Down);
            this.keyboard.UpPressed += Move(Direction.Up);
        }

        private Action Move(Direction direction)
        {
            return
                () => { this.entity.WalkAndPushInDirection(direction); };
        }
    }
}
