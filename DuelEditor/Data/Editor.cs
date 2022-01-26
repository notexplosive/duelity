using Duel.Components;
using Duel.Data;
using DuelEditor.Components;
using Machina.Components;
using Machina.Data;
using Machina.Data.Layout;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Data
{
    public class Editor
    {
        private readonly Sokoban game;

        public Editor(Scene scene, Sokoban game)
        {
            this.game = game;
            var viewportPixels = new Point((Room.Size.X + 2) * Grid.TileSize, (Room.Size.Y + 2) * Grid.TileSize);
            var layout = LayoutNode.HorizontalParent("root", LayoutSize.Pixels(scene.camera.UnscaledViewportSize), LayoutStyle.Empty,
                LayoutNode.VerticalParent("left-sidebar", LayoutSize.StretchedBoth(), LayoutStyle.Empty),
                LayoutNode.VerticalParent("editor", LayoutSize.StretchedVertically(viewportPixels.X), LayoutStyle.Empty,
                    LayoutNode.Leaf("tile-editor", LayoutSize.Pixels(viewportPixels)),
                    LayoutNode.Leaf("bottom-section", LayoutSize.StretchedBoth())
                ),
                LayoutNode.VerticalParent("right-sidebar", LayoutSize.StretchedBoth(), LayoutStyle.Empty)
            );


            var layoutActors = new LayoutActors(scene, layout);

            BecomePane(layoutActors.GetActor("left-sidebar"));
            BecomePane(layoutActors.GetActor("right-sidebar"));
            BecomePane(layoutActors.GetActor("bottom-section"));
            BecomeTileEditor(layoutActors.GetActor("tile-editor"));
        }

        private void BecomeTileEditor(Actor actor)
        {
            game.SetRootActorPosition(actor.transform.Position);
            new TileEditor(actor, game.CurrentLevel);
            new EditorPanner(actor, game);
        }

        private void BecomePane(Actor sidebarActor)
        {
            new NinepatchRenderer(sidebarActor, MachinaClient.DefaultStyle.windowSheet, NinepatchSheet.GenerationDirection.Inner);
        }
    }
}
