using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class MovementRenderer : BaseComponent
    {
        private readonly EntityRenderInfo renderInfo;
        private readonly Entity entity;
        private readonly TweenChain tween = new TweenChain();
        private float moveAnimationDelay;

        public bool IsMoving => this.moveAnimationDelay > 0;

        public MovementRenderer(Actor actor, Entity entity) : base(actor)
        {
            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.entity = entity;
            this.entity.PositionChanged += OnPositionChanged;
            this.entity.MoveFailed += BumpAnimation;
            this.entity.Nudged += BumpAnimation;
        }

        public override void Update(float dt)
        {
            this.tween.Update(dt);

            if (!this.tween.IsDone())
            {
                this.moveAnimationDelay = 0.05f;
            }
            else
            {
                this.moveAnimationDelay -= dt;
            }
        }

        private void OnPositionChanged(MoveType moveType, Point previousPosition)
        {
            if (moveType == MoveType.Warp)
            {
                this.tween.Clear();
                // Position snap is handled elsewhere.
            }

            if (moveType == MoveType.Walk)
            {
                StartWalkTween(this.renderInfo.TileToLocalPosition(previousPosition), this.renderInfo.TileToLocalPosition(this.entity.Position));
            }

            if (moveType == MoveType.Jump)
            {
                StartJumpTween(this.renderInfo.TileToLocalPosition(previousPosition), this.renderInfo.TileToLocalPosition(this.entity.Position));
            }

            if (moveType == MoveType.Charge)
            {
                StartChargeTween(this.renderInfo.TileToLocalPosition(previousPosition), this.renderInfo.TileToLocalPosition(this.entity.Position));
            }
        }

        private void StartChargeTween(Vector2 previousWorldPos, Vector2 targetWorldPos)
        {
            this.renderInfo.SnapPositionToGrid();
            var displacement = previousWorldPos - targetWorldPos;
            this.renderInfo.renderOffsetTweenable.setter(displacement);

            this.tween.Clear();
            this.tween.AppendVectorTween(Vector2.Zero, 0.05f * displacement.Length() / Grid.TileSize, EaseFuncs.QuadraticEaseIn, this.renderInfo.renderOffsetTweenable);
            this.entity.BusySignal.Add(new BusyFunction("ChargeTween", this.tween.IsDone));
        }

        public void BumpAnimation(Direction direction)
        {
            this.tween.Clear();
            this.tween.AppendVectorTween(direction.ToPoint().ToVector2() * 20, 0.05f, EaseFuncs.CubicEaseIn, this.renderInfo.renderOffsetTweenable);
            this.tween.AppendVectorTween(Vector2.Zero, 0.20f, EaseFuncs.CubicEaseOut, this.renderInfo.renderOffsetTweenable);
            this.entity.BusySignal.Add(new BusyFunction("BumpTween", this.tween.IsDone));
        }

        private void StartWalkTween(Vector2 previousWorldPos, Vector2 targetWorldPos)
        {
            this.renderInfo.SnapPositionToGrid();
            this.renderInfo.renderOffsetTweenable.setter(previousWorldPos - targetWorldPos);
            this.tween.Clear();
            this.tween.AppendVectorTween(Vector2.Zero, 0.10f, EaseFuncs.CubicEaseIn, this.renderInfo.renderOffsetTweenable);
            this.entity.BusySignal.Add(new BusyFunction("MoveTween", this.tween.IsDone));
        }

        private void StartJumpTween(Vector2 previousWorldPos, Vector2 targetWorldPos)
        {
            this.renderInfo.SnapPositionToGrid();
            var displacement = previousWorldPos - targetWorldPos;
            this.renderInfo.renderOffsetTweenable.setter(displacement);
            this.tween.Clear();

            this.tween.AppendVectorTween(Vector2.Zero, 0.10f * displacement.Length() / Grid.TileSize, EaseFuncs.EaseInBack, this.renderInfo.renderOffsetTweenable);
            this.entity.BusySignal.Add(new BusyFunction("JumpTween", this.tween.IsDone));
        }
    }
}
