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
        private readonly BufferedKeyboardListener keyboard;

        public UseGun(Actor actor, Entity entity, Level level) : base(actor)
        {
            this.solidProvider = new LevelSolidProvider(level);
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;
            keyboard.ActionPressed += Shoot;
        }

        public void Shoot()
        {
            var bullet = CreateBullet();

            if (bullet.HitAtLeastOneThing)
            {
                foreach (var hitLocation in bullet.HitLocationsReversed())
                {
                    this.solidProvider.ApplyHitAt(hitLocation, this.entity.FacingDirection);
                }
            }

            MachinaClient.Print("Bang!");
        }

        public FiredBullet CreateBullet()
        {
            return new FiredBullet(this.entity.Position, this.entity.FacingDirection, this.solidProvider);
        }
    }
}
