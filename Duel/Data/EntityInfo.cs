using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class EntityInfo
    {
        public Entity Entity { get; }
        public BusySignal BusySignal { get; }

        public EntityInfo(Entity entity, BusySignal levelBusySignal)
        {
            Entity = entity;
            BusySignal = levelBusySignal.MakeChild();
        }
    }
}
