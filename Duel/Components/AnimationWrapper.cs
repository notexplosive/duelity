using Machina.Data;

namespace Duel.Components
{
    public class AnimationWrapper
    {
        public AnimationWrapper()
        {
        }

        public AnimationWrapper(IFrameAnimation animation)
        {
            Animation = animation;
        }

        public static AnimationWrapper None = new AnimationWrapper();

        public IFrameAnimation Animation { get; }
    }
}
