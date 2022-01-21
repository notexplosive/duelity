using Duel.Components;
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
        public event Action LeftPressed;
        public event Action RightPressed;
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action ActionPressed;

        public KeyboardListener(Actor actor) : base(actor)
        {
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (state == ButtonState.Pressed)
            {
                if (key == Keys.Left || key == Keys.A)
                {
                    LeftPressed?.Invoke();
                }

                if (key == Keys.Right || key == Keys.D)
                {
                    RightPressed?.Invoke();
                }

                if (key == Keys.Up || key == Keys.W)
                {
                    UpPressed?.Invoke();
                }

                if (key == Keys.Down || key == Keys.S)
                {
                    DownPressed?.Invoke();
                }

                if (key == Keys.Z || key == Keys.Space)
                {
                    ActionPressed?.Invoke();
                }
            }
        }
    }
}
