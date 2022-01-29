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
    public class NpcRenderer : BaseComponent
    {
        private readonly SpriteSheet npcSheet;
        private readonly int frame;
        private float timer;

        public NpcRenderer(Actor actor, NpcSprite sprite) : base(actor)
        {
            this.npcSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("npcs-sheet");
            this.frame = (int)sprite * 2;

            RequireComponent<EntityRenderInfo>().DisableDebugGraphic();
        }

        public override void Update(float dt)
        {
            this.timer += dt * 6;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.npcSheet.DrawFrame(spriteBatch, this.frame + (int)(this.timer % 2), transform);
        }
    }
}
