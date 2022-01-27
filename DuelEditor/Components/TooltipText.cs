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
    public class TooltipText : BaseComponent
    {
        private readonly List<string> textBuffer = new List<string>();
        private BoundedTextRenderer textRenderer;

        public TooltipText(Actor actor) : base(actor)
        {
            this.textRenderer = RequireComponent<BoundedTextRenderer>();
        }

        public override void Update(float dt)
        {
            this.textRenderer.Text = string.Join("\n", this.textBuffer);
            this.textBuffer.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Add(string text)
        {
            this.textBuffer.Add(text);
        }
    }
}
