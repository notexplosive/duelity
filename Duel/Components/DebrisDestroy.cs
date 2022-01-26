using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class DebrisDestroy : BaseComponent
    {
        private readonly TweenChain tween = new TweenChain();
        private readonly SpriteRenderer spriteRenderer;
        private readonly TweenAccessors<float> opacity;
        private readonly TweenAccessors<float> scale;

        public DebrisDestroy(Actor actor, DestroyType destroyType) : base(actor)
        {
            this.spriteRenderer = RequireComponent<SpriteRenderer>();

            this.opacity = new TweenAccessors<float>(1);
            this.scale = new TweenAccessors<float>(1);

            var multi = this.tween.AppendMulticastTween();

            if (destroyType == DestroyType.Break)
            {
                var scaleChannel = multi.AddChannel();
                scaleChannel.AppendFloatTween(1.2f, 0.15f, EaseFuncs.QuadraticEaseOut, this.scale);
                scaleChannel.AppendFloatTween(0.8f, 0.05f, EaseFuncs.QuadraticEaseIn, this.scale);
                scaleChannel.AppendFloatTween(1f, 0.05f, EaseFuncs.QuadraticEaseOut, this.scale);

                var opacityChannel = multi.AddChannel();
                opacityChannel.AppendWaitTween(0.15f);
                opacityChannel.AppendFloatTween(0, 0.5f, EaseFuncs.CubicEaseIn, this.opacity);
            }
            else if (destroyType == DestroyType.Fall)
            {
                var opacityChannel = multi.AddChannel();
                opacityChannel.AppendFloatTween(0, 0.25f, EaseFuncs.QuadraticEaseIn, this.opacity);

                var scaleChannel = multi.AddChannel();
                scaleChannel.AppendFloatTween(0, 0.5f, EaseFuncs.QuadraticEaseIn, this.scale);
            }
        }

        public override void Update(float dt)
        {
            this.tween.Update(dt);
            this.spriteRenderer.color.A = (byte)(255 * this.opacity.CurrentValue);
            this.spriteRenderer.scale = this.scale.CurrentValue;
        }
    }
}
