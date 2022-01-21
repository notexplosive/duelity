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
    public class KeyboardListener : BaseComponent
    {
        private readonly BusySignal busySignal;

        public Action BufferedAction { get; private set; }

        public event Action LeftPressed;
        public event Action RightPressed;
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action ActionPressed;

        public KeyboardListener(Actor actor, BusySignal busySignal) : base(actor)
        {
            this.busySignal = busySignal;
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (state == ButtonState.Pressed)
            {
                if (key == Keys.Left || key == Keys.A)
                {
                    DoOrBuffer(LeftPressed);
                }

                if (key == Keys.Right || key == Keys.D)
                {
                    DoOrBuffer(RightPressed);
                }

                if (key == Keys.Up || key == Keys.W)
                {
                    DoOrBuffer(UpPressed);
                }

                if (key == Keys.Down || key == Keys.S)
                {
                    DoOrBuffer(DownPressed);
                }

                if (key == Keys.Z || key == Keys.Space)
                {
                    DoOrBuffer(ActionPressed);
                }
            }
        }

        public override void Update(float dt)
        {
            if (BufferedAction != null && this.busySignal.IsFree())
            {
                BufferedAction?.Invoke();
                BufferedAction = null;
            }
        }

        public void DoOrBuffer(Action action)
        {
            if (this.busySignal.IsFree())
            {
                action?.Invoke();
            }
            else
            {
                BufferedAction = action;
            }
        }
    }
}
