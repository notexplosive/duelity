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
        private readonly TweenAccessors<Vector2> renderOffset;
        private readonly TweenChain tween = new TweenChain();

        public EntityRenderer(Actor actor, LevelRenderer levelRenderer, Entity entity) : base(actor)
        {
            this.levelRenderer = levelRenderer;
            this.entity = entity;
            this.renderOffset = new TweenAccessors<Vector2>(Vector2.Zero);
            SnapPositionToGrid();

            this.entity.PositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(MoveType moveType, Point previousPosition)
        {
            if (moveType == MoveType.Warp)
            {
                this.tween.Clear();
                SnapPositionToGrid();
            }

            if (moveType == MoveType.Walk)
            {
                StartMoveTween(this.levelRenderer.TileToLocalPosition(previousPosition), this.levelRenderer.TileToLocalPosition(this.entity.Position));
            }

            if (moveType == MoveType.Jump)
            {
                StartJumpTween(this.levelRenderer.TileToLocalPosition(this.entity.Position));
            }
        }

        private void StartMoveTween(Vector2 previousWorldPos, Vector2 targetWorldPos)
        {
            this.tween.Clear();
            SnapPositionToGrid();
            this.renderOffset.setter(previousWorldPos - targetWorldPos);
            this.tween.AppendVectorTween(Vector2.Zero, 0.25f, EaseFuncs.CubicEaseIn, renderOffset);
            this.entity.BusySignal.Add(new BusyFunction("MoveTween", this.tween.IsDone));
        }

        private void StartJumpTween(Vector2 targetWorldPos)
        {
            MachinaClient.Print("Jump todo");
        }

        public void SnapPositionToGrid()
        {
            transform.LocalPosition = this.levelRenderer.TileToLocalPosition(this.entity.Position);
        }

        public override void Update(float dt)
        {
            this.tween.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(transform.Position + this.renderOffset.getter(), 20), 20, Color.Orange, 20, transform.Depth);
        }
    }
}
