using Duel.Data;
using Machina.Components;
using Machina.Engine;
using System;

namespace Duel.Components
{
    public class UseGun : BaseComponent
    {
        private readonly Entity entity;
        private readonly LevelSolidProvider solidProvider;
        private readonly KeyboardListener keyboard;

        public UseGun(Actor actor, Entity entity, Level level) : base(actor)
        {
            this.solidProvider = new LevelSolidProvider(level);
            this.keyboard = RequireComponent<KeyboardListener>();
            this.entity = entity;
            keyboard.ActionPressed += Shoot;
        }

        private void Shoot()
        {
            var bullet = new FiredBullet(this.entity.Position, this.entity.FacingDirection, this.solidProvider);

            if (bullet.HitSomethingHittable)
            {
                this.solidProvider.ApplyHitAt(bullet.HitLocation, this.entity.FacingDirection);
            }

            MachinaClient.Print("Bang!", bullet.HitSomethingHittable ? bullet.HitLocation.ToString() : "missed", "blocked:", bullet.WasBlocked, "entity:", bullet.HittableEntity);
        }
    }
}
