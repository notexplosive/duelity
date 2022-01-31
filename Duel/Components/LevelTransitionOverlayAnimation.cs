using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class LevelTransitionOverlayAnimation : BaseComponent
    {
        private TweenAccessors<float> circleRadiusTweenable;
        private TweenAccessors<float> imageSizeTweenable;
        private PlayerTag.Type currentPlayer;
        private PlayerTag.Type nextPlayer;
        private PlayerTag.Type displayedPlayer;
        private Color backgroundColor;
        private SpriteSheet spriteSheet;
        private TweenChain tween;

        public LevelTransitionOverlayAnimation(Actor actor, Scene originalScene, Action midwayCallback = null) : base(actor)
        {
            this.circleRadiusTweenable = new TweenAccessors<float>(0);
            this.imageSizeTweenable = new TweenAccessors<float>(0);
            this.currentPlayer = DuelGameCartridge.Instance.GetCurrentChapter().Player;
            this.nextPlayer = DuelGameCartridge.Instance.PeekNextChapter().Player;
            this.displayedPlayer = this.currentPlayer;

            this.backgroundColor = new Color(255, 89, 68);
            this.spriteSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("silos-sheet");

            this.tween = new TweenChain();

            var multi_phase1 = this.tween.AppendMulticastTween();

            var width = this.actor.scene.camera.UnscaledViewportSize.X;

            multi_phase1.AddChannel()
                .AppendFloatTween(width, 0.25f, EaseFuncs.CubicEaseIn, this.circleRadiusTweenable)
                ;

            multi_phase1.AddChannel()
                .AppendWaitTween(0.25f)
                .AppendFloatTween(1f, 0.5f, EaseFuncs.EaseOutBack, this.imageSizeTweenable)
                .AppendWaitTween(0.25f)
                .AppendFloatTween(25f, 1f, EaseFuncs.CubicEaseIn, this.imageSizeTweenable)
                .AppendCallback(() => { this.displayedPlayer = this.nextPlayer; })
                .AppendFloatTween(0.8f, 0.8f, EaseFuncs.CubicEaseIn, this.imageSizeTweenable)
                .AppendFloatTween(1f, 0.2f, EaseFuncs.CubicEaseOut, this.imageSizeTweenable)
                ;

            var sceneLayers = this.actor.scene.sceneLayers;
            Scene gameScene = null;

            this.tween.AppendWaitTween(0.25f);

            this.tween.AppendCallback(() =>
            {
                midwayCallback?.Invoke();

                var i = sceneLayers.IndexOf(originalScene);
                gameScene = new Scene(sceneLayers, MachinaClient.GlobalFrameStep);
                sceneLayers.Set(i, gameScene);
                DuelGameCartridge.Instance.GetCurrentChapterAndIncrement().Load(gameScene);
                gameScene.Freeze();
            });

            this.tween.AppendWaitTween(0.25f);

            var multi_phase2 = this.tween.AppendMulticastTween();

            multi_phase2.AddChannel()
                .AppendWaitTween(0.25f)
                .AppendFloatTween(0, 1f, EaseFuncs.CubicEaseOut, this.circleRadiusTweenable)
                ;

            multi_phase2.AddChannel()
                .AppendFloatTween(0, 1f, EaseFuncs.EaseInBack, this.imageSizeTweenable)
                ;


            this.tween.AppendCallback(() =>
            {
                sceneLayers.RemoveScene(this.actor.scene);
                gameScene.Unfreeze();
            });
        }

        public override void Update(float dt)
        {
            this.tween.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var center = this.actor.scene.camera.ViewportCenter;
            var radius = this.circleRadiusTweenable.CurrentValue;
            spriteBatch.DrawCircle(new CircleF(center, radius), 50, this.backgroundColor, radius, new Depth(200));
            this.spriteSheet.DrawFrame(spriteBatch, (int)this.displayedPlayer, center, this.imageSizeTweenable.CurrentValue, 0, XYBool.False, new Depth(100), Color.White);
        }
    }
}
