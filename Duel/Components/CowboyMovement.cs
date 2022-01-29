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
        private readonly Sokoban game;

        public CowboyMovement(Actor actor, Entity entity, LevelSolidProvider solidProvider, Sokoban game) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;
            this.solidProvider = solidProvider;
            this.game = game;
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

                    if (this.solidProvider.HasTagAt<UnfilledWater>(this.entity.Position))
                    {
                        // play splash sound
                    }

                    if (this.solidProvider.HasTagAt<Ravine>(this.entity.Position))
                    {
                        // play fall down ravine sound
                        break;
                    }
                }
                this.solidProvider.ApplyHitAt(chargeHit.RammedPosition, chargeHit.Direction);
            }

            var fellInWater = this.solidProvider.HasTagAt<UnfilledWater>(this.entity.Position);
            var fellInRavine = this.solidProvider.HasTagAt<Ravine>(this.entity.Position);

            if (fellInWater || fellInRavine)
            {
                this.actor.Visible = false;

                if (fellInWater)
                {
                    // play gloop sound
                    yield return new WaitSeconds(1f);
                }

                if (fellInRavine)
                {
                    yield return new WaitSeconds(1f);
                }
                this.game.RestartRoom();
            }
        }

        public void ResumeMoveFromOldInstance(Entity playerFromPreviousRoom, Point newPlayerPosition)
        {
            this.entity.JumpToPosition(newPlayerPosition);
        }
    }
}
