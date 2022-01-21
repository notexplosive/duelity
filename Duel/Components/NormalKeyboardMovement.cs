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

            this.keyboard.LeftPressed += Move(new Point(-1, 0));
            this.keyboard.RightPressed += Move(new Point(1, 0));
            this.keyboard.DownPressed += Move(new Point(0, 1));
            this.keyboard.UpPressed += Move(new Point(0, -1));
        }

        private Action Move(Point relativePosition)
        {
            return
                () => { this.entity.WalkToPosition(this.entity.Position + relativePosition); };
        }
    }
}
