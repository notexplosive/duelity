using Machina.Engine;
using Microsoft.Xna.Framework.Audio;

namespace Duel.Data.Dialog
{
    public class DialogBlip
    {
        private readonly string soundAssetName;
        private readonly float pitchModification;
        private readonly float volume;
        private SoundEffectInstance soundEffect;

        public DialogBlip(string soundAssetName, float volume, float pitchModification)
        {
            this.soundAssetName = soundAssetName;
            this.pitchModification = pitchModification;
            this.volume = volume;
        }

        public void Play()
        {
            if (this.soundEffect == null)
            {
                this.soundEffect = MachinaClient.Assets.GetSoundEffectInstance(this.soundAssetName);
            }

            this.soundEffect.Stop();
            this.soundEffect.Pitch = this.pitchModification;
            this.soundEffect.Volume = this.volume;
            this.soundEffect.Play();
        }

        public static DialogBlip Default
        {
            get
            {
                return new DialogBlip("default_blip", 0.5f, 0);
            }
        }
    }
}