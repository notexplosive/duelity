using Duel.Components;
using Duel.Data.Dialog;
using Machina.Components;
using Machina.Data;
using Machina.Data.Layout;
using Machina.Engine;
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
        private BoundedTextRenderer textRenderer;
        private SpriteSheet portraitSheet;
        private SpriteFont font;


        public DialogueBoxRenderer(Actor actor) : base(actor)
        {
            this.dialogueRunner = RequireComponent<DialogueRunner>();
            this.dialogueRunner.StartedRun += ClearText;

            var layout = LayoutNode.VerticalParent("screen", LayoutSize.Pixels(actor.scene.camera.UnscaledViewportSize), new LayoutStyle(margin: new Point(64, 64)),
                LayoutNode.StretchedSpacer(),
                LayoutNode.HorizontalParent("dialogueBox", LayoutSize.StretchedHorizontally(200), new LayoutStyle(margin: new Point(20, 20), padding: 15),
                    LayoutNode.VerticalParent("speaker", LayoutSize.StretchedVertically(128), new LayoutStyle(padding: 5),
                        LayoutNode.Leaf("speaker-face", LayoutSize.Pixels(128, 128)),
                        LayoutNode.Leaf("speaker-name", LayoutSize.StretchedBoth())
                    ),
                    LayoutNode.Leaf("text", LayoutSize.StretchedBoth())
                )
            );

            this.bakedLayout = layout.Bake();

            var child = this.actor.transform.AddActorAsChild("textActor");

            this.font = MachinaClient.Assets.GetSpriteFont("DialogueFont");

            new BoundingRect(actor, bakedLayout.GetNode("text").Size);
            this.textRenderer = new BoundedTextRenderer(actor, "Hello world", this.font, Color.White);

            actor.transform.Depth = 50;

            actor.transform.Position = bakedLayout.GetNode("text").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition;

            this.portraitSheet = MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("portraits");

            MachinaClient.Print(bakedLayout.GetNode("text").PositionRelativeToRoot);

            new LayoutActors(actor.scene, layout);
        }

        private void ClearText()
        {
            this.textRenderer.Text = "Test";
        }

        public override void Update(float dt)
        {

        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.dialogueRunner.PendingEvent is Say say)
            {
                var dialogBox = bakedLayout.GetNode("dialogueBox").Rectangle;
                dialogBox.Location += this.actor.scene.camera.UnscaledPosition.ToPoint();

                spriteBatch.FillRectangle(dialogBox, Color.Black, new Depth(100));
                this.portraitSheet.DrawFrame(spriteBatch, say.Speaker.PortraitIndex, bakedLayout.GetNode("speaker-face").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition + new Vector2(64), new Depth(50));
                spriteBatch.DrawString(this.font, say.Speaker.Name, bakedLayout.GetNode("speaker-name").PositionRelativeToRoot.ToVector2() + this.actor.scene.camera.UnscaledPosition, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, new Depth(50));

                this.textRenderer.Text = this.dialogueRunner.CurrentText;
            }
        }
    }
}
