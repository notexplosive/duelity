using Duel.Components;
using Duel.Data.Dialog;
using Machina.Components;
using Machina.Data;
using Machina.Data.Layout;
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
    public class DialogueBoxRenderer : BaseComponent
    {
        private readonly BakedLayout bakedLayout;
        private readonly DialogueRunner dialogueRunner;
        private readonly BoundedTextRenderer textRenderer;
        private readonly SpriteSheet portraitSheet;
        private readonly SpriteFont font;
        private TweenAccessors<float> faceHeight;
        private TweenAccessors<float> namePositionX;
        private TweenChain faceTween;
        private TweenChain nameTween;
        private string cachedName;
        private int cachedPortraitIndex;

        public DialogueBoxRenderer(Actor actor) : base(actor)
        {
            this.font = MachinaClient.Assets.GetSpriteFont("DialogueFont");

            this.dialogueRunner = RequireComponent<DialogueRunner>();
            this.dialogueRunner.StartedRun += ClearText;

            var layout = LayoutNode.VerticalParent("screen", LayoutSize.Pixels(actor.scene.camera.UnscaledViewportSize), new LayoutStyle(margin: new Point(64, 64)),
                LayoutNode.StretchedSpacer(),
                LayoutNode.HorizontalParent("dialogueBox", LayoutSize.StretchedHorizontally(200), new LayoutStyle(margin: new Point(20, 20), padding: 15),
                    LayoutNode.VerticalParent("speaker", LayoutSize.StretchedVertically(128), new LayoutStyle(padding: 5),
                        LayoutNode.Leaf("speaker-face", LayoutSize.Pixels(128, 128))
                    ),
                    LayoutNode.Leaf("text", LayoutSize.StretchedBoth())
                ),
                LayoutNode.Leaf("speaker-name", LayoutSize.StretchedHorizontally(this.font.LineSpacing))
            );

            this.bakedLayout = layout.Bake();

            var child = this.actor.transform.AddActorAsChild("textActor");


            new BoundingRect(actor, bakedLayout.GetNode("text").Size);
            this.textRenderer = new BoundedTextRenderer(actor, "Hello world", this.font, Color.White);

            actor.transform.Depth = 50;

            actor.transform.Position = bakedLayout.GetNode("text").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition;

            this.portraitSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("portraits");

            new LayoutActors(actor.scene, layout);


            this.faceHeight = new TweenAccessors<float>(0);
            this.namePositionX = new TweenAccessors<float>(0);

            this.faceTween = new TweenChain()
                .AppendFloatTween(-10f, 0.15f, EaseFuncs.QuadraticEaseOut, this.faceHeight)
                .AppendFloatTween(0f, 0.15f, EaseFuncs.QuadraticEaseIn, this.faceHeight);

            this.nameTween = new TweenChain();

            this.nameTween = new TweenChain()
                .AppendFloatTween(10f, 0.10f, EaseFuncs.CubicEaseIn, this.namePositionX)
                .AppendFloatTween(0f, 0.10f, EaseFuncs.CubicEaseIn, this.namePositionX);
        }

        private void ClearText(IDialogEvent dialogEvent)
        {
            if (dialogEvent is Say say)
            {
                this.textRenderer.Text = "";

                if (this.cachedName != say.Speaker.Name)
                {
                    this.nameTween.Refresh();
                }

                if (this.cachedPortraitIndex != say.Speaker.PortraitIndex)
                {
                    this.faceTween.Refresh();
                }

                this.cachedName = say.Speaker.Name;
                this.cachedPortraitIndex = say.Speaker.PortraitIndex;
            }
        }

        public override void Update(float dt)
        {
            this.faceTween.Update(dt);
            this.nameTween.Update(dt);
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.dialogueRunner.PendingEvent is Say say)
            {
                var dialogBox = bakedLayout.GetNode("dialogueBox").Rectangle;
                dialogBox.Location += this.actor.scene.camera.UnscaledPosition.ToPoint();

                spriteBatch.FillRectangle(dialogBox, new Color(10, 10, 10), new Depth(100));
                this.portraitSheet.DrawFrame(spriteBatch, say.Speaker.PortraitIndex, bakedLayout.GetNode("speaker-face").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition + new Vector2(64) + new Vector2(0, this.faceHeight.CurrentValue), new Depth(50));

                void DrawSpeakerName(Color color, Vector2 offset, Depth depthOffset)
                {
                    spriteBatch.DrawString(this.font, say.Speaker.Name, bakedLayout.GetNode("speaker-name").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition + new Vector2(20, -this.font.LineSpacing / 2) + offset + new Vector2(this.namePositionX.CurrentValue, 0), color, 0, Vector2.Zero, 1f, SpriteEffects.None, new Depth(50) + depthOffset);
                }

                DrawSpeakerName(Color.White, Vector2.Zero, 0);
                DrawSpeakerName(Color.Black, new Vector2(3), 1);
            }

            this.textRenderer.Text = this.dialogueRunner.CurrentText;
        }
    }
}
