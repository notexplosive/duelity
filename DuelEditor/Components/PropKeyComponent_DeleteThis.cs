using DuelEditor.Components;
using DuelEditor.Data;
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
    public class PropKeyComponent_DeleteThis : BaseComponent
    {
        private readonly TemplateSelection templateSelection;
        private readonly TextureRenderer textureRenderer;

        public PropKeyComponent_DeleteThis(Actor actor, TemplateSelection templateSelection) : base(actor)
        {
            this.templateSelection = templateSelection;
            this.textureRenderer = RequireComponent<TextureRenderer>();
        }

        public override void Update(float dt)
        {
            if (!this.templateSelection.IsInPropMode)
            {
                this.textureRenderer.Opacity = 0.5f;
            }
            else
            {
                this.textureRenderer.Opacity = 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
