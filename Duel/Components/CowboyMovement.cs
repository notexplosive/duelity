using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Duel.Components
{
    public class CowboyMovement : BaseComponent, IPlayerMovementComponent
    {
        private readonly BufferedKeyboardListener keyboard;
        private readonly Entity entity;
        private readonly LevelSolidProvider solidProvider;

        public CowboyMovement(Actor actor, Entity entity, LevelSolidProvider solidProvider) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;
            this.solidProvider = solidProvider;
            this.keyboard.LeftPressed += Move(Direction.Left, true);
            this.keyboard.RightPressed += Move(Direction.Right, true);
            this.keyboard.DownPressed += Move(Direction.Down, true);
            this.keyboard.UpPressed += Move(Direction.Up, true);
        }

        private Action Move(Direction direction, bool wasInitiatedByKeyboard)
        {
            return
                () =>
                {
                    var projectedMove = CreateCharge(direction);
                    var moveAnimation = this.actor.scene.StartCoroutine(ChargeCoroutine(projectedMove));
                    this.entity.BusySignal.Add(new BusyFunction("Charging", moveAnimation.IsDone));
                };
        }

        public Charge CreateCharge(Direction direction)
        {
            return new Charge(entity.Position, direction, this.solidProvider);
        }

        public IEnumerator<ICoroutineAction> ChargeCoroutine(Charge projectedMove)
        {
            foreach (var chargeHit in projectedMove.Path)
            {
                if (chargeHit.StandingPosition != this.entity.Position)
                {
                    this.entity.ChargeToPosition(chargeHit.StandingPosition, chargeHit.Direction);
                    yield return new WaitUntil(() => this.entity.BusySignal.Exists("ChargeTween"));
                    yield return new WaitUntil(this.entity.BusySignal.GetSpecific("ChargeTween").IsFree);
                }
                this.solidProvider.ApplyHitAt(chargeHit.RammedPosition, chargeHit.Direction);
            }
        }

        public void ResumeMoveFromOldInstance(Entity playerFromPreviousRoom, Point newPlayerPosition)
        {
            this.entity.JumpToPosition(newPlayerPosition);
        }
    }
}
