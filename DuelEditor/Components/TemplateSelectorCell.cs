using Duel.Data;
using DuelEditor.Components;
using DuelEditor.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Components
{
    public class TemplateSelectorCell : BaseComponent
    {
        private readonly BoundingRect boundingRect;
        private readonly Clickable clickable;
        private readonly IEntityOrTileTemplate template;
        private readonly TemplateSelection templateSelectorCell;

        public TemplateSelectorCell(Actor actor, IEntityOrTileTemplate template, TemplateSelection templateSelectorCell) : base(actor)
        {
            this.template = template;
            this.templateSelectorCell = templateSelectorCell;

            this.boundingRect = RequireComponent<BoundingRect>();
            this.clickable = RequireComponent<Clickable>();

            this.clickable.OnClick += OnClick;
        }

        private void OnClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                templateSelectorCell.Primary = this.template;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.clickable.IsHovered)
            {
                spriteBatch.DrawRectangle(boundingRect.Rect, Color.Blue, 3f, transform.Depth - 10);
            }
        }
    }
}
