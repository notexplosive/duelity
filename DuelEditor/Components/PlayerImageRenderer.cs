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
    public class PlayerImageRenderer : BaseComponent
    {
        private readonly PlayerTag.Type player;
        private readonly SpriteSheet characters;

        public PlayerImageRenderer(Actor actor, PlayerTag.Type player) : base(actor)
        {
            this.player = player;
            this.characters = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.characters.DrawFrame(spriteBatch, (int)this.player * 6, transform);
        }
    }
}
