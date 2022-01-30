using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public class TitleScreen : IChapter
    {
        private Scene gameScene;

        public void Load(Scene gameScene)
        {
            this.gameScene = gameScene;
            gameScene.sceneLayers.BackgroundColor = new Color(255, 89, 68);
            gameScene.StartCoroutine(IntroCoroutine());
        }

        private IEnumerator<ICoroutineAction> IntroCoroutine()
        {
            yield return new WaitSeconds(1f);

            DuelGameCartridge.Instance.MusicPlayer.PlayTrack(TrackName.Title);
            var notexplosive = PutTextureActor("title_screen_notexplosive");

            yield return WaitBeat();

            var andrfw = PutTextureActor("title_screen_andrfw");

            yield return WaitBeat();

            var ggj = PutTextureActor("title_screen_ggj");

            yield return WaitBeat();

            notexplosive.Destroy();
            andrfw.Destroy();
            ggj.Destroy();

            yield return WaitBeat();

            PutTextureActor("title_screen_sheriff");

            yield return WaitHalfBeat();

            PutTextureActor("title_screen_renegade");

            yield return WaitHalfBeat();

            PutTextureActor("title_screen_cowboy");

            yield return WaitHalfBeat();

            PutTextureActor("title_screen_knight");

            yield return WaitHalfBeat();

            PutTextureActor("title_screen_duelity");

            yield return WaitBeat();

            PutTextureActor("title_screen_press_space");
        }

        private ICoroutineAction WaitBeat()
        {
            return new WaitSeconds(2.2222222222f);
        }

        private ICoroutineAction WaitHalfBeat()
        {
            return new WaitSeconds(1.1111111111f);
        }

        public Actor PutTextureActor(string textureName)
        {
            var actor = this.gameScene.AddActor(textureName);

            new TextureRenderer(actor, MachinaClient.Assets.GetTexture(textureName));
            return actor;
        }
    }
}
