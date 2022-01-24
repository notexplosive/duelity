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
    public class LeverRenderer : BaseComponent
    {
        private readonly SignalState signalState;
        private readonly SpriteSheet spriteSheet;
        private readonly LeverImages images;
        private readonly SignalColor color;

        public LeverRenderer(Actor actor, SignalColor signalColor, SignalState signalState) : base(actor)
        {
            RequireComponent<EntityRenderInfo>().DisableDebugGraphic();
            this.signalState = signalState;
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet");
            this.images = new LeverImages(signalColor);
            this.color = signalColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var frame = this.signalState.IsOn(this.color) ? this.images.RightImage : this.images.LeftImage;
            this.spriteSheet.DrawFrame(spriteBatch, (int)frame, transform.Position, transform.Depth + 10);
        }
    }
}
