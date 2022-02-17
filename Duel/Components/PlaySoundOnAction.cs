using Machina.Components;
using Machina.Engine;

namespace Duel.Components
{
    public class PlaySoundOnAction : BaseComponent
    {
        public PlaySoundOnAction(Actor actor, string soundEffectName) : base(actor)
        {
            this.keyboard = RequireComponent<BufferedKeyboardListener>();
            SoundEffectName = soundEffectName;
            this.keyboard.ActionPressed += PlaySound;
        }

        private void PlaySound()
        {
            MachinaClient.Assets.GetSoundEffectInstance(SoundEffectName).Play();
        }

        private readonly BufferedKeyboardListener keyboard;
        public string SoundEffectName { get; }
    }
}
