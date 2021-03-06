using Duel.Data;
using Machina.Components;
using Machina.Engine;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Duel.Components
{
    public class UseGun : BaseComponent
    {
        private readonly Entity entity;
        private readonly LevelSolidProvider solidProvider;
        private readonly BufferedKeyboardListener keyboard;
        public event Action<FiredBullet> Fired;

        public UseGun(Actor actor, Entity entity, Level level) : base(actor)
        {
            this.solidProvider = new LevelSolidProvider(level);
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            this.entity = entity;
            keyboard.ActionPressed += Shoot;

        }

        public void Shoot()
        {
            DuelGameCartridge.PlaySound("shoot", stopFirst: true);
            var bullet = CreateBullet();

            if (bullet.HitAtLeastOneThing)
            {
                foreach (var hitLocation in bullet.HitLocationsReversed())
                {
                    this.solidProvider.ApplyHitAt(hitLocation, this.entity.FacingDirection);
                }
            }

            Fired?.Invoke(bullet);
        }

        public FiredBullet CreateBullet()
        {
            return new FiredBullet(this.entity.Position, this.entity.FacingDirection, this.solidProvider);
        }
    }
}
