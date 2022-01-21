using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public delegate bool FuncBool();

    public class BusySignal
    {
        private readonly List<BusyFunction> busyFunctions = new List<BusyFunction>();

        public bool IsBusy()
        {
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

        public void Add(BusyFunction busyFunction)
        {
            this.busyFunctions.Add(busyFunction);
        }

        public IEnumerable<BusyFunction> PendingBusyFunctions()
        {
            foreach (var busyFunction in this.busyFunctions)
            {
                if (busyFunction.IsBusy())
                {
                    yield return busyFunction;
                }
            }
        }
    }
}
