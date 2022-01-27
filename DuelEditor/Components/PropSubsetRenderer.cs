using Duel.Components;
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
    public class PropSubsetRenderer : BaseComponent
    {
        private Texture2D texture;

        public PropSubsetRenderer(Actor actor, Texture2D texture) : base(actor)
        {
            this.texture = texture;
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            var sourceRect = new Rectangle(Point.Zero, new Point(Grid.TileSize));
            var destRect = new Rectangle(transform.Position.ToPoint(), new Point(Grid.TileSize));

            spriteBatch.Draw(this.texture, destRect, sourceRect, Color.White, 0, new Vector2(Grid.TileSize) / 2, SpriteEffects.None, transform.Depth);
        }
    }
}
