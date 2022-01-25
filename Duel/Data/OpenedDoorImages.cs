using Duel.Components;

namespace Duel.Data
{
    public class OpenedDoorImages : ISignalableImages
    {
        private readonly ClosedDoorImages oppositeImpl;

        public OpenedDoorImages(SignalColor color)
        {
            this.oppositeImpl = new ClosedDoorImages(color);
        }

        public SignalColor SignalColor => this.oppositeImpl.SignalColor;
        public EntityFrame OnImage => this.oppositeImpl.OffImage;
        public EntityFrame OffImage => this.oppositeImpl.OnImage;
    }
}
