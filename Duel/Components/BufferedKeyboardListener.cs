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
    public class BufferedKeyboardListener : BaseComponent
    {
        private readonly BusySignal busySignal;
        private readonly DirectionalButtons heldDirections = new DirectionalButtons();
        private float heldKeyTimer;

        private float bufferedActionTimer;
        private Action bufferedActionImpl;
        private bool isAllowedToStart;

        public Action BufferedAction
        {
            get => this.bufferedActionImpl;
            private set
            {
                this.bufferedActionTimer = 0.1f;
                this.bufferedActionImpl = value;
            }
        }

        public event Action LeftPressed;
        public event Action RightPressed;
        public event Action UpPressed;
        public event Action DownPressed;
        public event Action ActionPressed;

        public BufferedKeyboardListener(Actor actor, BusySignal busySignal) : base(actor)
        {
            this.busySignal = busySignal;
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (this.isAllowedToStart)
            {
                if (state == ButtonState.Pressed)
                {
                    this.heldKeyTimer = 0.2f;

                    DoOrBuffer(DirectionToAction(KeyToDirection(key)));

                    if (key == Keys.Z || key == Keys.Space && modifiers.None)
                    {
                        DoOrBuffer(ActionPressed);
                    }
                }

                var pressed = state == ButtonState.Pressed;

                if (KeyToDirection(key) == Direction.Left)
                {
                    this.heldDirections.left = pressed;
                }

                if (KeyToDirection(key) == Direction.Right)
                {
                    this.heldDirections.right = pressed;
                }

                if (KeyToDirection(key) == Direction.Up)
                {
                    this.heldDirections.up = pressed;
                }

                if (KeyToDirection(key) == Direction.Down)
                {
                    this.heldDirections.down = pressed;
                }
            }
        }

        public override void Update(float dt)
        {
            if (!this.heldDirections.HasPressed())
            {
                // weird edge case involving room transitions
                this.isAllowedToStart = true;
            }

            if (this.bufferedActionTimer > 0)
            {
                this.bufferedActionTimer -= dt;
            }
            else
            {
                BufferedAction = null;
            }

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
                        this.heldKeyTimer -= dt;
                        if (this.heldKeyTimer < 0)
                        {
                            DoOrBuffer(DirectionToAction(this.heldDirections.GetFirstDirection()));
                            this.heldKeyTimer = 0.05f;
                        }
                    }
                }
            }
        }

        private Direction KeyToDirection(Keys key)
        {
            if (key == Keys.Left || key == Keys.A)
            {
                return Direction.Left;
            }

            if (key == Keys.Right || key == Keys.D)
            {
                return Direction.Right;
            }

            if (key == Keys.Up || key == Keys.W)
            {
                return Direction.Up;
            }

            if (key == Keys.Down || key == Keys.S)
            {
                return Direction.Down;
            }

            return Direction.None;
        }

        private Action DirectionToAction(Direction direction)
        {
            if (direction == Direction.Up)
            {
                return UpPressed;
            }
            if (direction == Direction.Down)
            {
                return DownPressed;
            }
            if (direction == Direction.Left)
            {
                return LeftPressed;
            }
            if (direction == Direction.Right)
            {
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
