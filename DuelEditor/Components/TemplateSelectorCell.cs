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
        private readonly TooltipText tooltipText;
        private readonly ITemplate template;
        private readonly TemplateSelection selection;

        public TemplateSelectorCell(Actor actor, ITemplate template, TemplateSelection selection, TooltipText tooltipText) : base(actor)
        {
            this.template = template;
            this.selection = selection;

            this.boundingRect = RequireComponent<BoundingRect>();
            this.clickable = RequireComponent<Clickable>();
            this.tooltipText = tooltipText;

            this.clickable.OnClick += OnClick;
        }

        public override void Update(float dt)
        {
            if (this.selection.Primary == this.template)
            {
                var entityOrTileName = "tile";
                if (this.selection.Primary is EntityTemplate)
                {
                    entityOrTileName = "entity";
                }

                var info = this.selection.Primary.Tags.ToString();

                if (this.selection.Primary is PropTemplate prop)
                {
                    entityOrTileName = "prop";
                    info = prop.Name;
                }

                this.tooltipText.Add($"selected {entityOrTileName}: {info}");
            }
        }

        private void OnClick(MouseButton button)
        {
            if (button == MouseButton.Left)
            {
                selection.Primary = this.template;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.clickable.IsHovered)
            {
                spriteBatch.DrawRectangle(boundingRect.Rect, Color.Blue, 3f, transform.Depth - 10);
            }

            if (this.selection.Primary == this.template)
            {
                var rect = boundingRect.Rect;
                rect.Inflate(5, 5);
                spriteBatch.DrawRectangle(rect, Color.Cyan, 5f, transform.Depth - 8);
            }
        }
    }
}
