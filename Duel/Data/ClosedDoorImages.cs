using Duel.Components;

namespace Duel.Data
{
    public class ClosedDoorImages : ISignalableImages
    {
        public ClosedDoorImages(SignalColor color)
        {
            SignalColor = color;

            if (color == SignalColor.Blue)
            {
                OffImage = EntityFrame.BlueDoor;
                OnImage = EntityFrame.BlueDoorOpen;
            }

            if (color == SignalColor.Red)
            {
                OffImage = EntityFrame.RedDoor;
                OnImage = EntityFrame.RedDoorOpen;
            }

            if (color == SignalColor.Yellow)
            {
                OffImage = EntityFrame.YellowDoor;
                OnImage = EntityFrame.YellowDoorOpen;
            }
        }

        public SignalColor SignalColor { get; }
        public EntityFrame OnImage { get; }
        public EntityFrame OffImage { get; }
        public float Scale => 1f;
    }
}
