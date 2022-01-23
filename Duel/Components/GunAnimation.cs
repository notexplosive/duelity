using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class GunAnimation : BaseComponent
    {
        private readonly PlayerCharacterRenderer renderer;
        private readonly UseGun useGun;
        private readonly Grid grid;
        private readonly Entity entity;
        private float animationTimer;
        private FiredBullet pendingBullet;
        private const float AnimationDuration = 0.15f;

        public GunAnimation(Actor actor, Entity entity, Grid grid) : base(actor)
        {
            this.renderer = RequireComponent<PlayerCharacterRenderer>();
            this.useGun = RequireComponent<UseGun>();
            this.grid = grid;
            this.entity = entity;

            this.useGun.Fired += OverrideAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("miranda-shoot"));
        }

        private Action<FiredBullet> OverrideAnimation(IFrameAnimation frameAnimation)
        {
            return (bullet) =>
            {
                this.pendingBullet = bullet;
                this.entity.Nudge(this.entity.FacingDirection.Opposite);
                this.animationTimer = AnimationDuration;
                this.entity.BusySignal.Add(new BusyFunction("ShootAnimation", () => this.animationTimer < 0));
                this.renderer.AnimationOverride = new AnimationWrapper(frameAnimation);
            };
        }

        public override void Update(float dt)
        {
            this.animationTimer -= dt;

            if (this.animationTimer < 0)
            {
                this.pendingBullet = null;
                this.renderer.AnimationOverride = AnimationWrapper.None;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.pendingBullet != null)
            {
                var startPos = this.grid.TileToLocalPosition(this.pendingBullet.StartPosition);
                var endPos = this.grid.TileToLocalPosition(this.pendingBullet.LastHitLocation);

                var thicknessFactor = EaseFuncs.EaseOutBack(this.animationTimer / AnimationDuration);
                spriteBatch.DrawLine(startPos, endPos, Color.White, 5f * thicknessFactor, transform.Depth + 1);
                spriteBatch.DrawCircle(new CircleF(endPos, 8f * thicknessFactor), 16, Color.White, 8f * thicknessFactor, transform.Depth + 1);
            }
        }
    }
}
