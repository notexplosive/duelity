using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class EntityInfo
    {
        public Entity Entity { get; }
        public BusySignal BusySignal { get; } = new BusySignal();

        public EntityInfo(Entity entity)
        {
            Entity = entity;
        }
    }
}
