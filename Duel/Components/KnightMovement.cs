using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class KnightMovement : BaseComponent
    {
        private readonly SolidProvider solidProvider;
        private readonly Entity entity;
        private readonly BufferedKeyboardListener keyboard;
        public Direction LongLeg { get; private set; } = Direction.None;
        public event Action MoveStarted;
        public event Action MoveCanceled;
        public event Action MoveFailed;
        public event Action MoveComplete;

        public KnightMovement(Actor actor, Entity entity, SolidProvider solidProvider) : base(actor)
        {
            this.solidProvider = solidProvider;
            this.entity = entity;
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.keyboard.LeftPressed += BuildCallback(Direction.Left);
            this.keyboard.RightPressed += BuildCallback(Direction.Right);
            this.keyboard.DownPressed += BuildCallback(Direction.Down);
            this.keyboard.UpPressed += BuildCallback(Direction.Up);
        }

        private Action BuildCallback(Direction direction)
        {
            return () =>
            {
                if (LongLeg == Direction.None)
                {
                    LongLeg = direction;
                    MoveStarted?.Invoke();
                }
                else
                {
                    if (LongLeg == direction)
                    {
                        this.entity.Nudge(direction);
                        return;
                    }

                    var isCancel = (LongLeg.ToPoint() + direction.ToPoint()) == Point.Zero;

                    if (isCancel)
                    {
                        this.entity.Nudge(direction);
                        LongLeg = Direction.None;
                        MoveCanceled?.Invoke();
                    }
                    else
                    {
                        var offset = LongLeg.ToPoint() + LongLeg.ToPoint() + direction.ToPoint();
                        var targetPosition = this.entity.Position + offset;

                        if (!this.solidProvider.IsSolidAt(targetPosition))
                        {
                            MoveComplete?.Invoke();
                            this.entity.JumpToPosition(targetPosition);
                        }
                        else
                        {
                            MoveFailed?.Invoke();
                            this.entity.Nudge(LongLeg);
                        }

                        LongLeg = Direction.None;
                    }
                }
            };
        }
    }
}
