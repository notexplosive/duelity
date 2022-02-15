using Machina.Components;
using Machina.Data;
using Machina.Data.TextRendering;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class Finale : IChapter
    {
        public PlayerTag.Type Player => PlayerTag.Type.Sheriff;

        private Scene scene;
        private Actor pendingActor;

        public Dialogue Dialogue { get; private set; }

        public void Load(Scene scene)
        {
            DuelGameCartridge.Instance.MusicPlayer.StopAll();
            this.scene = scene;
            Dialogue = new Dialogue(scene, new Level());

            Dialogue.InvokeHappened += OnInvoke;

            scene.StartCoroutine(CinematicThenCredits());
        }

        private IEnumerator<ICoroutineAction> CinematicThenCredits()
        {
            var conversation = DuelGameCartridge.Instance.Screenplay.GetConversation("comic_book");

            yield return Dialogue.StartDialogue(conversation);

            this.pendingActor.Destroy();

            SpawnCredits();
        }

        private void SpawnCredits()
        {
            var actor = this.scene.AddActor("credits");
            new BoundingRect(actor, scene.camera.UnscaledViewportSize);

            var credits = new StringBuilder();

            credits.AppendLine("DUEL-ITY");
            credits.AppendLine("By NotExplosive and andrfw");
            credits.AppendLine();
            credits.AppendLine("Made in 10 days for Global Game Jam 2022");
            credits.AppendLine("Hosted by the Portland Indie Game Squad");
            credits.AppendLine();
            credits.AppendLine("Programming & Level Design by NotExplosive");
            credits.AppendLine("Graphics & Music by andrfw");
            credits.AppendLine("Font is Mochiy Pop P One by FONTDASU");
            credits.AppendLine();
            credits.AppendLine("Play more of our games at");
            credits.AppendLine("NOTEXPLOSIVE.NET and ANDRFW.COM");

            this.scene.sceneLayers.BackgroundColor = Color.Black; //new Color(255, 89, 68);

            new BoundedFormattedTextRenderer(actor, Alignment.Center, Overflow.Ignore, formattedText: FormattedText.FromString(credits.ToString(), MachinaClient.Assets.GetSpriteFont("CreditsFont"), Color.White));
            actor.transform.Position += new Vector2(0, scene.camera.UnscaledViewportSize.Y * 1.25f);

            var tween = new TweenChainComponent(actor);

            tween.AddMoveTween(Vector2.Zero, 2, EaseFuncs.EaseOutBack);
        }

        private void OnInvoke(string eventName)
        {
            this.pendingActor?.Destroy();
            this.pendingActor = PutTextureActor(eventName);
        }

        public Actor PutTextureActor(string textureName)
        {
            var actor = this.scene.AddActor(textureName);

            new TextureRenderer(actor, MachinaClient.Assets.GetTexture(textureName));
            return actor;
        }
    }
}
