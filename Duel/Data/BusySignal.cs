using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public delegate bool TrueWhenDone();

    public class BusySignal
    {
        private readonly List<BusyFunction> busyFunctions = new List<BusyFunction>();
        private readonly BusySignal parent = null;
        private bool stoppedAcceptingNewFunctions;

        public BusySignal()
        {

        }

        private BusySignal(BusySignal parent)
        {
            this.parent = parent;
        }

        public bool IsFree()
        {
            return !IsBusy();
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

        public void StopAcceptingNewFunctions()
        {
            this.stoppedAcceptingNewFunctions = true;
        }

        public bool ParentIsBusy()
        {
            return this.parent != null && this.parent.IsBusy();
        }

        public void Add(BusyFunction busyFunction)
        {
            if (this.stoppedAcceptingNewFunctions)
            {
                return;
            }

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

        public bool Exists(string name)
        {
            foreach (var busyFunction in this.busyFunctions)
            {
                if (busyFunction.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public BusySignal MakeChild()
        {
            return new BusySignal(this);
        }

        public BusyFunction GetSpecific(string name)
        {
            foreach (var busyFunction in this.busyFunctions)
            {
                if (busyFunction.Name == name)
                {
                    return busyFunction;
                }
            }

            throw new Exception($"No busy function called {name} found");
        }
    }
}
