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
    public class SignalableRenderer : BaseComponent
    {
        private readonly SignalState signalState;
        private readonly SpriteSheet spriteSheet;
        private readonly ISignalableImages images;
        private readonly SignalColor color;

        public SignalableRenderer(Actor actor, ISignalableImages images, SignalState signalState) : base(actor)
        {
            RequireComponent<EntityRenderInfo>().DisableDebugGraphic();
            this.signalState = signalState;
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet");
            this.color = images.SignalColor;
            this.images = images;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var frame = this.signalState.IsOn(this.color) ? this.images.OnImage : this.images.OffImage;
            this.spriteSheet.DrawFrame(spriteBatch, (int)frame, transform.Position, this.images.Scale, 0, XYBool.False, transform.Depth + 10, Color.White);
        }
    }
}
