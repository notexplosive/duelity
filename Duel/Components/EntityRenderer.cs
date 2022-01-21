using Duel.Data;
using Machina.Components;
using Machina.Engine;
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

        public EntityRenderer(Actor actor, LevelRenderer levelRenderer, Entity entity) : base(actor)
        {
            this.levelRenderer = levelRenderer;
            this.entity = entity;
            SnapPositionToGrid();

            this.entity.PositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(MoveType moveType, Point previousPosition)
        {
            if (moveType == MoveType.Warp)
            {
                SnapPositionToGrid();
            }

            if (moveType == MoveType.Walk)
            {
                StartMoveTween(this.levelRenderer.TileToLocalPosition(this.entity.Position));
            }

            if (moveType == MoveType.Jump)
            {
                StartJumpTween(this.levelRenderer.TileToLocalPosition(this.entity.Position));
            }
        }

        private void StartJumpTween(Vector2 targetWorldPos)
        {
            MachinaClient.Print("Jump todo");
        }

        private void StartMoveTween(Vector2 targetWorldPos)
        {
            MachinaClient.Print("Walk todo");
        }

        public void SnapPositionToGrid()
        {
            transform.LocalPosition = this.levelRenderer.TileToLocalPosition(this.entity.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(transform.Position, 20), 20, Color.Orange, 20, transform.Depth);
        }
    }
}
