using Duel.Components;
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
    public class MiasmaRenderer : BaseComponent
    {
        private readonly SpriteFrame spriteFrame;
        private readonly NoiseBasedRNG random;
        private XYBool flip;
        private float timer;

        public MiasmaRenderer(Actor actor) : base(actor)
        {
            this.spriteFrame = new SpriteFrame(MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet"), (int)EntityFrame.Miasma);
            this.random = new NoiseBasedRNG((uint)MachinaClient.RandomDirty.Next());
        }

        public override void Update(float dt)
        {
            this.timer -= dt;

            if (this.timer < 0)
            {
                CalculateFlip();
                this.timer = 1f / 4;
            }
        }

        public void CalculateFlip()
        {
            var oldFlip = this.flip;

            // I wish XYBool had an equality comparison
            while (this.flip.x == oldFlip.x && this.flip.y == oldFlip.y)
            {
                this.flip = new XYBool(this.random.NextBool(), this.random.NextBool());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // dangit i wish spriteFrame had a draw function
            this.spriteFrame.spriteSheet.DrawFrame(spriteBatch, (int)EntityFrame.Miasma, transform.Position, 1f, 0f, this.flip, transform.Depth - 100, Color.White);
        }
    }
}
