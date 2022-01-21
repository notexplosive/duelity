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
        private readonly DirectionalButtons heldDirections = new DirectionalButtons();
        private float waitTimer;

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
                this.waitTimer = 0.2f;

                if (key == Keys.Left || key == Keys.A)
                {
                    DoOrBuffer(LeftPressed);
                    this.heldDirections.left = true;
                }

                if (key == Keys.Right || key == Keys.D)
                {
                    DoOrBuffer(RightPressed);
                    this.heldDirections.right = true;
                }

                if (key == Keys.Up || key == Keys.W)
                {
                    DoOrBuffer(UpPressed);
                    this.heldDirections.up = true;
                }

                if (key == Keys.Down || key == Keys.S)
                {
                    DoOrBuffer(DownPressed);
                    this.heldDirections.down = true;
                }

                if (key == Keys.Z || key == Keys.Space)
                {
                    DoOrBuffer(ActionPressed);
                }
            }

            if (state == ButtonState.Released)
            {
                if (key == Keys.Left || key == Keys.A)
                {
                    this.heldDirections.left = false;
                }

                if (key == Keys.Right || key == Keys.D)
                {
                    this.heldDirections.right = false;
                }

                if (key == Keys.Up || key == Keys.W)
                {
                    this.heldDirections.up = false;
                }

                if (key == Keys.Down || key == Keys.S)
                {
                    this.heldDirections.down = false;
                }
            }
        }

        public override void Update(float dt)
        {
            if (this.busySignal.IsFree())
            {
                if (BufferedAction != null)
                {
                    BufferedAction?.Invoke();
                    BufferedAction = null;
                }
                else
                {
                    if (this.heldDirections.HasPressed())
                    {
                        this.waitTimer -= dt;
                        if (this.waitTimer < 0)
                        {
                            DoOrBuffer(DirectionToAction(this.heldDirections.GetFirstDirection()));
                            this.waitTimer = 0.05f;
                        }
                    }
                }
            }
        }

        private Action DirectionToAction(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return UpPressed;
                case Direction.Down:
                    return DownPressed;
                case Direction.Left:
                    return LeftPressed;
                case Direction.Right:
                    return RightPressed;
            }

            return null;
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
