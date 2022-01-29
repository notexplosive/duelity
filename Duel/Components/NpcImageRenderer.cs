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
    public class NpcImageRenderer : BaseComponent
    {
        private readonly SpriteSheet npcSheet;
        private readonly int frame;

        public NpcImageRenderer(Actor actor, NpcSprite sprite) : base(actor)
        {
            this.npcSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("npcs-sheet");
            this.frame = (int)sprite;
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.npcSheet.DrawFrame(spriteBatch, this.frame, transform);
        }
    }
}
