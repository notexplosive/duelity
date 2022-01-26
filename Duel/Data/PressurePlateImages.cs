using Duel.Components;

namespace Duel.Data
{
    public class PressurePlateImages : ISignalableImages
    {
        public PressurePlateImages(SignalColor color)
        {
            SignalColor = color;

            if (color == SignalColor.Blue)
            {
                ReleasedImage = EntityFrame.BluePlateUp;
                PressedImage = EntityFrame.BluePlateDown;
            }

            if (color == SignalColor.Red)
            {
                ReleasedImage = EntityFrame.RedPlateUp;
                PressedImage = EntityFrame.RedPlateDown;
            }

            if (color == SignalColor.Yellow)
            {
                ReleasedImage = EntityFrame.YellowPlateUp;
                PressedImage = EntityFrame.YellowPlateDown;
            }
        }

        public EntityFrame ReleasedImage { get; }
        public EntityFrame PressedImage { get; }

        public EntityFrame OnImage => PressedImage;
        public EntityFrame OffImage => ReleasedImage;

        public SignalColor SignalColor { get; }
    }
}
