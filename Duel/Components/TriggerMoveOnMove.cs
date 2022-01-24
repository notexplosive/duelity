using Duel.Data;
using Machina.Components;
using Machina.Engine;
using Microsoft.Xna.Framework;

namespace Duel.Components
{
    public class TriggerMoveOnMove : BaseComponent
    {
        private readonly Entity entity;
        private readonly Level level;

        public TriggerMoveOnMove(Actor actor, Entity entity, Level level) : base(actor)
        {
            this.entity = entity;
            this.entity.PositionChanged += OnMoved;
            this.level = level;
        }

        public override void OnDeleteFinished()
        {
            this.entity.PositionChanged -= OnMoved;
        }

        private void OnMoved(MoveType movetype, Point previousposition)
        {
            if (movetype != MoveType.Warp)
            {
                this.level.EntityJustSteppedOff(previousposition);
            }
        }
    }
}