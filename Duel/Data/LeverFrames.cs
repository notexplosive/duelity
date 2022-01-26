using Duel.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class LeverFrames : ISignalableImages
    {
        public LeverFrames(SignalColor color)
        {
            SignalColor = color;

            if (color == SignalColor.Blue)
            {
                LeftImage = EntityFrame.BlueLeverLeft;
                RightImage = EntityFrame.BlueLeverRight;
            }

            if (color == SignalColor.Red)
            {
                LeftImage = EntityFrame.RedLeverLeft;
                RightImage = EntityFrame.RedLeverRight;
            }

            if (color == SignalColor.Yellow)
            {
                LeftImage = EntityFrame.YellowLeverLeft;
                RightImage = EntityFrame.YellowLeverRight;
            }
        }

        public EntityFrame LeftImage { get; }
        public EntityFrame RightImage { get; }

        public EntityFrame OnImage => LeftImage;
        public EntityFrame OffImage => RightImage;

        public SignalColor SignalColor { get; }
    }
}
