using Duel.Data;
using DuelEditor.Components;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Components
{
    public class EntityImageRenderer : BaseComponent
    {
        private readonly SpriteSheet spriteSheet;
        private readonly EntityFrameSet entityFrameSet;

        public EntityImageRenderer(Actor actor, EntityFrameSet entityFrameSet) : base(actor)
        {
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet");
            this.entityFrameSet = entityFrameSet;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.spriteSheet.DrawFrame(spriteBatch, this.entityFrameSet.Normal, transform);
        }
    }

    public class TileImageRenderer : BaseComponent
    {
        private readonly SpriteSheet spriteSheet;
        private readonly TileFrame imageFrame;

        public TileImageRenderer(Actor actor, TileImageTag.TileImage image) : base(actor)
        {
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("tiles-sheet");
            this.imageFrame = TileImageToImageFrame(image);
        }

        private TileFrame TileImageToImageFrame(TileImageTag.TileImage image)
        {
            switch (image)
            {
                case TileImageTag.TileImage.Wall:
                    return TileFrame.Wall;
                case TileImageTag.TileImage.Hook:
                    return TileFrame.Hook;
                case TileImageTag.TileImage.Water:
                    return TileFrame.WaterAloneNub;
                case TileImageTag.TileImage.Ravine:
                    return TileFrame.RavineAloneNub;
                case TileImageTag.TileImage.Bramble:
                    return TileFrame.Bramble0;
                case TileImageTag.TileImage.Bridge:
                    return TileFrame.Bridge;
                default:
                    return TileFrame.Wall;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.spriteSheet.DrawFrame(spriteBatch, (int)this.imageFrame, transform);
        }
    }
}
