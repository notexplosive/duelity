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
    public class NormalKeyboardMovement : BaseComponent
    {
        private readonly Entity entity;
        private readonly KeyboardListener keyboard;

        public NormalKeyboardMovement(Actor actor, Entity entity) : base(actor)
        {
            this.keyboard = RequireComponent<KeyboardListener>();
            this.entity = entity;

            this.keyboard.LeftPressed += Move(Direction.Left);
            this.keyboard.RightPressed += Move(Direction.Right);
            this.keyboard.DownPressed += Move(Direction.Down);
            this.keyboard.UpPressed += Move(Direction.Up);
        }

        private Action Move(Direction direction)
        {
            return
                () => { this.entity.WalkInDirection(direction); };
        }
    }
}
