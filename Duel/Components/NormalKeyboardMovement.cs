using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class NormalKeyboardMovement : BaseComponent, IPlayerMovementComponent
    {
        private readonly Entity entity;
        private readonly BufferedKeyboardListener keyboard;
        private readonly string walkSoundName;

        public NormalKeyboardMovement(Actor actor, Entity entity, string walkSoundName) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;

            this.keyboard.LeftPressed += Move(Direction.Left);
            this.keyboard.RightPressed += Move(Direction.Right);
            this.keyboard.DownPressed += Move(Direction.Down);
            this.keyboard.UpPressed += Move(Direction.Up);

            this.walkSoundName = walkSoundName;

            this.entity.PositionChanged += PlayWalkSound;
        }

        private void PlayWalkSound(Entity mover, MoveType moveType, Point previousPosition)
        {
            DuelGameCartridge.PlaySound(this.walkSoundName, stopFirst: true);
        }

        public void ResumeMoveFromOldInstance(Entity playerFromPreviousRoom, Point newPlayerPosition)
        {
            this.entity.FacingDirection = playerFromPreviousRoom.FacingDirection;
            Move(playerFromPreviousRoom.FacingDirection)();
        }

        private Action Move(Direction direction)
        {
            return
                () =>
                {
                    this.entity.WalkAndPushInDirection(direction);
                };
        }
    }
}
