using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Duel.Components
{
    public class EntityRenderer : BaseComponent
    {
        private readonly LevelRenderer levelRenderer;
        private readonly Entity entity;
        public readonly TweenAccessors<Vector2> renderOffsetTweenable;

        public EntityRenderer(Actor actor, LevelRenderer levelRenderer, Entity entity) : base(actor)
        {
            this.levelRenderer = levelRenderer;
            this.entity = entity;
            this.renderOffsetTweenable = new TweenAccessors<Vector2>(Vector2.Zero);
            SnapPositionToGrid();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(RenderPosition, 20), 20, Color.Orange, 20, transform.Depth);
        }

        public void SnapPositionToGrid()
        {
            transform.LocalPosition = TileToLocalPosition(this.entity.Position);
        }

        public Vector2 RenderPosition => transform.Position + this.renderOffsetTweenable.getter();

        public Vector2 TileToLocalPosition(Point position)
        {
            return this.levelRenderer.TileToLocalPosition(position);
        }
    }
}
