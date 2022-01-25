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
    public class EntityRenderInfo : BaseComponent
    {
        private readonly Grid grid;
        private readonly Entity entity;
        public readonly TweenAccessors<Vector2> renderOffsetTweenable;
        private bool skipDebug;

        public EntityRenderInfo(Actor actor, Grid grid, Entity entity) : base(actor)
        {
            this.grid = grid;
            this.entity = entity;
            this.renderOffsetTweenable = new TweenAccessors<Vector2>(Vector2.Zero);
            SnapPositionToGrid();

            this.entity.PositionChanged += SnapPositionIfWarp;
        }

        public void DisableDebugGraphic()
        {
            this.skipDebug = true;
        }

        private void SnapPositionIfWarp(Entity mover, MoveType moveType, Point previousPosition)
        {
            if (moveType == MoveType.Warp)
            {
                SnapPositionToGrid();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.skipDebug)
            {
                spriteBatch.DrawCircle(new CircleF(RenderPosition, 20), 20, Color.OrangeRed, 20, transform.Depth);
            }
        }

        public void SnapPositionToGrid()
        {
            transform.LocalPosition = TileToLocalPosition(this.entity.Position);
        }

        public Vector2 RenderPosition => transform.Position + this.renderOffsetTweenable.getter();

        public Vector2 TileToLocalPosition(Point position)
        {
            return this.grid.TileToLocalPosition(position);
        }
    }
}
