using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class PlayerDirectionRenderer : BaseComponent
    {
        private readonly EntityRenderInfo renderInfo;
        private readonly Entity entity;
        private readonly Color color;

        public PlayerDirectionRenderer(Actor actor, Entity entity, Color color) : base(actor)
        {
            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.entity = entity;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var previousDirectionVector = this.entity.FacingDirection.Previous.ToPoint().ToVector2();
            var nextDirectionVector = this.entity.FacingDirection.Next.ToPoint().ToVector2();
            var oppositeDirectionVector = this.entity.FacingDirection.Next.Next.ToPoint().ToVector2();
            var directionVector = this.entity.FacingDirection.ToPoint().ToVector2();


            var arrowStart = this.renderInfo.RenderPosition + directionVector * Grid.TileSize * 0.4f;
            var arrowEnd = this.renderInfo.RenderPosition + directionVector * Grid.TileSize * 0.7f;
            var thickness = 3f;
            var depth = transform.Depth - 10;
            var arrowHeadLength = 10;

            spriteBatch.DrawLine(arrowStart, arrowEnd, this.color, thickness, depth);
            spriteBatch.DrawCircle(new CircleF(arrowEnd, thickness / 2), 8, this.color, thickness / 2, depth);
            spriteBatch.DrawLine(arrowEnd, arrowEnd + oppositeDirectionVector * arrowHeadLength + nextDirectionVector * arrowHeadLength, this.color, thickness, depth);
            spriteBatch.DrawLine(arrowEnd, arrowEnd + oppositeDirectionVector * arrowHeadLength + previousDirectionVector * arrowHeadLength, this.color, thickness, depth);
        }
    }
}
