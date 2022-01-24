using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class SignalState
    {
        public HashSet<SignalColor> onSignals = new HashSet<SignalColor>();

        public void TurnOn(SignalColor signalColor)
        {
            this.onSignals.Add(signalColor);
        }

        public void TurnOff(SignalColor signalColor)
        {
            this.onSignals.Remove(signalColor);
        }

        public void Toggle(SignalColor signalColor)
        {
            if (IsOn(signalColor))
            {
                TurnOff(signalColor);
            }
            else
            {
                TurnOn(signalColor);
            }
        }

        public bool IsOn(SignalColor signalColor)
        {
            return this.onSignals.Contains(signalColor);
        }
    }
}
