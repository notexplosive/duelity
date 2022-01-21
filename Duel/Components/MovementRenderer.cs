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

        public MovementRenderer(Actor actor, Entity entity) : base(actor)
        {
            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.entity = entity;
            this.entity.PositionChanged += OnPositionChanged;
        }

        public override void Update(float dt)
        {
            this.tween.Update(dt);
        }

        private void OnPositionChanged(MoveType moveType, Point previousPosition)
        {
            if (moveType == MoveType.Warp)
            {
                this.tween.Clear();
                this.renderInfo.SnapPositionToGrid();
            }

            if (moveType == MoveType.Walk)
            {
                StartMoveTween(this.renderInfo.TileToLocalPosition(previousPosition), this.renderInfo.TileToLocalPosition(this.entity.Position));
            }

            if (moveType == MoveType.Jump)
            {
                StartJumpTween(this.renderInfo.TileToLocalPosition(this.entity.Position));
            }
        }

        private void StartMoveTween(Vector2 previousWorldPos, Vector2 targetWorldPos)
        {
            this.tween.Clear();
            this.renderInfo.SnapPositionToGrid();
            this.renderInfo.renderOffsetTweenable.setter(previousWorldPos - targetWorldPos);
            this.tween.AppendVectorTween(Vector2.Zero, 0.10f, EaseFuncs.CubicEaseIn, this.renderInfo.renderOffsetTweenable);
            this.entity.BusySignal.Add(new BusyFunction("MoveTween", this.tween.IsDone));
        }

        private void StartJumpTween(Vector2 targetWorldPos)
        {
            MachinaClient.Print("Jump todo");
        }
    }
}
