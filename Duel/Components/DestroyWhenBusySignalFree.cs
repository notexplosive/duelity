using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class DestroyWhenBusySignalFree : BaseComponent
    {
        private readonly BusySignal busySignal;
        private DestroyType destroyType;

        public DestroyWhenBusySignalFree(Actor actor, BusySignal busySignal, DestroyType destroyType) : base(actor)
        {
            this.busySignal = busySignal;
            this.busySignal.StopAcceptingNewFunctions();
            this.destroyType = destroyType;
        }

        public override void Update(float dt)
        {
            if (this.busySignal.IsFree())
            {
                var simpleEntityRenderer = this.actor.GetComponent<SimpleEntityRenderer>();
                if (simpleEntityRenderer != null)
                {
                    simpleEntityRenderer.MakeDebris(destroyType);
                }

                this.actor.Destroy();
            }
        }
    }
}
