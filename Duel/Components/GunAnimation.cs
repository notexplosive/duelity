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
    public class GunAnimation : BaseComponent
    {
        private readonly PlayerCharacterRenderer renderer;
        private readonly UseGun useGun;
        private readonly Entity entity;
        private float animationTimer;

        public GunAnimation(Actor actor, Entity entity) : base(actor)
        {
            this.renderer = RequireComponent<PlayerCharacterRenderer>();
            this.useGun = RequireComponent<UseGun>();
            this.entity = entity;

            this.useGun.Fired += OverrideAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("miranda-shoot"));
        }

        private Action OverrideAnimation(IFrameAnimation frameAnimation)
        {
            return () =>
            {
                this.animationTimer = 0.15f;
                this.entity.BusySignal.Add(new BusyFunction("ShootAnimation", () => this.animationTimer < 0));
                this.renderer.AnimationOverride = new AnimationWrapper(frameAnimation);
            };
        }

        public override void Update(float dt)
        {
            this.animationTimer -= dt;

            if (this.animationTimer < 0)
            {
                this.renderer.AnimationOverride = AnimationWrapper.None;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
