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

        public DestroyWhenBusySignalFree(Actor actor, BusySignal busySignal) : base(actor)
        {
            this.busySignal = busySignal;
            this.busySignal.StopAcceptingNewFunctions();
        }

        public override void Update(float dt)
        {
            if (this.busySignal.IsFree())
            {
                this.actor.Destroy();
            }
        }
    }
}
