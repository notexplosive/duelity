using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class KnightCharacterRenderer : BaseComponent
    {
        private readonly KnightMovement knightMovement;
        private readonly EntityRenderInfo renderInfo;
        private readonly Actor spriteActor;
        private readonly SpriteRenderer spriteRenderer;
        private readonly IFrameAnimation idleAnimation;
        private readonly IFrameAnimation hoverAnimation;
        private readonly IFrameAnimation landingAnimation;
        private TweenAccessors<float> knightHeightTweenable;
        private TweenChain tween;

        public KnightCharacterRenderer(Actor actor) : base(actor)
        {
            this.knightMovement = RequireComponent<KnightMovement>();
            this.knightMovement.MoveStarted += RiseUp;
            this.knightMovement.MoveComplete += UpAndDown;
            this.knightMovement.MoveCanceled += FallDown;
            this.knightMovement.MoveFailed += FallDown;

            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.renderInfo.DisableDebugGraphic();

            this.spriteActor = transform.AddActorAsChild("sprite");
            this.spriteRenderer = new SpriteRenderer(this.spriteActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet"));

            this.idleAnimation = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("bennigan-idle");
            this.hoverAnimation = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("bennigan-hover");
            this.landingAnimation = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("bennigan-land");

            this.spriteRenderer.SetAnimation(this.idleAnimation);
            this.spriteRenderer.FramesPerSecond = 10;
            spriteActor.transform.LocalPosition = new Vector2(0, -Grid.TileSize / 4);

            this.knightHeightTweenable = new TweenAccessors<float>(0);
            this.tween = new TweenChain();
        }

        private void FallDown()
        {
            this.tween.Clear();
            this.tween.AppendFloatTween(0, 0.15f, EaseFuncs.QuadraticEaseIn, this.knightHeightTweenable);
        }

        private void UpAndDown()
        {
            this.spriteRenderer.SetAnimation(this.landingAnimation);

            this.tween.Clear();
            this.tween.AppendFloatTween(40, 0.15f, EaseFuncs.QuadraticEaseOut, this.knightHeightTweenable);
            this.tween.AppendFloatTween(0, 0.10f, EaseFuncs.QuadraticEaseIn, this.knightHeightTweenable);
        }

        private void RiseUp()
        {
            if (this.knightMovement.LongLeg == Direction.Left)
            {
                this.spriteRenderer.FlipX = false;
            }

            if (this.knightMovement.LongLeg == Direction.Right)
            {
                this.spriteRenderer.FlipX = true;
            }

            this.spriteRenderer.SetAnimation(this.hoverAnimation);

            this.tween.Clear();
            this.tween.AppendFloatTween(10, 0.25f, EaseFuncs.CubicEaseIn, this.knightHeightTweenable);
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            // technically we don't need to do this, but if we're in framestep it looks weird if we don't
            BindSpritePosToRenderOffset();
        }

        public override void Update(float dt)
        {
            BindSpritePosToRenderOffset();
            this.tween.Update(dt);

            if (this.knightHeightTweenable.CurrentValue == 0)
            {
                this.spriteRenderer.SetAnimation(this.idleAnimation);
            }
        }

        private void BindSpritePosToRenderOffset()
        {
            this.spriteRenderer.SetOffset(RenderOffset());
            this.spriteActor.transform.LocalDepth = -(int)this.knightHeightTweenable.CurrentValue - 1;
        }

        private Vector2 RenderOffset()
        {
            return -this.renderInfo.renderOffsetTweenable.getter() + new Vector2(0, this.knightHeightTweenable.getter());
        }
    }
}
