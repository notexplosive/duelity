using Duel.Data;
using Machina.Components;
using Machina.Engine;

namespace Duel.Components
{
    public class RemoveEntityOnDestroy : BaseComponent
    {
        private readonly Level level;
        private readonly Entity entity;

        public RemoveEntityOnDestroy(Actor actor, Level level, Entity entity) : base(actor)
        {
            this.level = level;
            this.entity = entity;
        }

        public override void OnDeleteFinished()
        {
            this.level.RemoveEntity(this.entity);
        }
    }
}
