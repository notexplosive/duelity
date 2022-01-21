using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public delegate bool FuncBool();

    public class BusySignal
    {
        private readonly List<BusyFunction> busyFunctions = new List<BusyFunction>();
        private readonly BusySignal parent = null;

        public BusySignal()
        {

        }

        private BusySignal(BusySignal parent)
        {
            this.parent = parent;
        }

        public bool IsBusy()
        {
            if (ParentIsBusy())
            {
                return true;
            }

            foreach (var busyFunction in this.busyFunctions)
            {
                if (busyFunction.IsBusy())
                {
                    return true;
                }
            }

            // Clear busyFunctions list when we're totally unbusy'd
            this.busyFunctions.Clear();

            return false;
        }

        public bool ParentIsBusy()
        {
            return this.parent != null && this.parent.IsBusy();
        }

        public void Add(BusyFunction busyFunction)
        {
            this.busyFunctions.Add(busyFunction);
        }

        public IEnumerable<BusyFunction> PendingBusyFunctions()
        {
            if (ParentIsBusy())
            {
                foreach (var parentBusyFunction in this.parent.PendingBusyFunctions())
                {
                    yield return parentBusyFunction;
                }
            }

            foreach (var busyFunction in this.busyFunctions)
            {
                if (busyFunction.IsBusy())
                {
                    yield return busyFunction;
                }
            }
        }

        public BusySignal MakeChild()
        {
            return new BusySignal(this);
        }
    }
}
