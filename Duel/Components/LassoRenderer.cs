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
    public class LassoRenderer : BaseComponent
    {
        private readonly SpriteSheet spriteSheet;
        private readonly EntityRenderInfo renderInfo;
        private readonly Direction throwDirection;
        private readonly LassoProjectile lasso;
        private readonly EntityRenderInfo casterRenderInfo;
        private float timer;

        public LassoRenderer(Actor actor, LassoProjectile lasso, Actor caster) : base(actor)
        {
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet");
            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.renderInfo.DisableDebugGraphic();
            this.throwDirection = lasso.ThrowDirection;
            this.lasso = lasso;
            this.casterRenderInfo = caster.GetComponent<EntityRenderInfo>();
            this.timer = 0;
        }

        public override void Update(float dt)
        {
            if (!this.lasso.IsReturning)
            {
                this.timer += dt;
            }
            else
            {
                this.timer -= dt;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var clampedTimer = Math.Clamp(this.timer * 5, 0, 1);
            var easedClampedTimer = EaseFuncs.CubicEaseIn(clampedTimer);
            this.spriteSheet.DrawFrame(spriteBatch, 5, this.renderInfo.RenderPosition, easedClampedTimer, this.throwDirection.Radians(), XYBool.False, transform.Depth - 20, Color.White);

            if (clampedTimer > 0.25)
            {
                var lineStart = this.casterRenderInfo.RenderPosition + this.throwDirection.ToGridCellSizedVector();
                var lineEnd = this.renderInfo.RenderPosition - this.throwDirection.ToGridCellSizedVector() * easedClampedTimer;
                spriteBatch.DrawLine(lineStart, lineEnd, Color.Black, 6, transform.Depth - 20);
                spriteBatch.DrawLine(lineStart, lineEnd, new Color(102, 38, 58), 3, transform.Depth - 25);
            }
        }
    }
}
