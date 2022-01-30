using Duel.Components;

namespace Duel.Data
{
    public interface ISignalableImages
    {
        public EntityFrame OnImage { get; }
        public EntityFrame OffImage { get; }

        public SignalColor SignalColor { get; }
        public float Scale { get; }
    }
}
