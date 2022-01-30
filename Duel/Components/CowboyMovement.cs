using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private SoundEffectInstance walkSound;
        private SoundEffectInstance fellInRavineSound;

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

            this.walkSound = MachinaClient.Assets.GetSoundEffectInstance("horse");
            this.fellInRavineSound = MachinaClient.Assets.GetSoundEffectInstance("startled_cow");
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

        public override void Update(float dt)
        {
            var busySignal = this.entity.BusySignal;
            if (busySignal.Exists("Charging") && !busySignal.GetSpecific("Charging").IsFree() && this.actor.Visible)
            {
                this.walkSound.Play();
            }
            else
            {
                this.walkSound.Stop();
            }
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

            var fellInWater = this.solidProvider.HasTagAt<UnfilledWater>(this.entity.Position);
            var fellInRavine = this.solidProvider.HasTagAt<Ravine>(this.entity.Position);

            if (fellInWater || fellInRavine)
            {
                this.actor.Visible = false;

                var debrisActor = this.actor.scene.AddActor("DeadSprite");
                var debrisSpriteRenderer = new SpriteRenderer(debrisActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet"));
                debrisSpriteRenderer.FramesPerSecond = 0;
                debrisSpriteRenderer.SetFrame(16);
                debrisActor.transform.Position = transform.Position;
                debrisActor.transform.Depth = transform.Depth - 1;
                new DebrisDestroy(debrisActor, DestroyType.Fall);

                if (fellInWater)
                {
                    var sound = MachinaClient.Assets.GetSoundEffectInstance("splorp");
                    sound.Volume = 0.5f;
                    sound.Play();
                }

                if (fellInRavine)
                {
                    this.fellInRavineSound.Play();
                }

                yield return new WaitSeconds(1f);
                this.game.RestartRoom();
            }
        }

        public void ResumeMoveFromOldInstance(Entity playerFromPreviousRoom, Point newPlayerPosition)
        {
            this.entity.JumpToPosition(newPlayerPosition);
        }
    }
}
