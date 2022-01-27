using Duel.Components;
using Duel.Data;
using DuelEditor.Components;
using Machina.Components;
using Machina.Data;
using Machina.Data.Layout;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Data
{
    public class Editor
    {
        private readonly Sokoban game;
        private readonly TemplateSelection templateSelection;

        public Editor(Scene scene, Sokoban game)
        {
            this.game = game;
            this.templateSelection = new TemplateSelection();
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

            var bakedLayout = layout.Bake();

            BecomeTileSelectorPane(layoutActors.GetActor("left-sidebar"), bakedLayout.GetNode("left-sidebar"), scene);
            BecomeBasicPane(layoutActors.GetActor("right-sidebar"));
            BecomeBasicPane(layoutActors.GetActor("bottom-section"));
            BecomeTileEditor(layoutActors.GetActor("tile-editor"));
        }

        private void BecomeTileEditor(Actor actor)
        {
            game.SetRootActorPosition(actor.transform.Position);
            new Hoverable(actor);
            new TileEditor(actor, game.CurrentLevel, this.templateSelection);
            new EditorPanner(actor, game);
        }

        private void BecomeBasicPane(Actor sidebarActor)
        {
            new NinepatchRenderer(sidebarActor, MachinaClient.DefaultStyle.windowSheet, NinepatchSheet.GenerationDirection.Inner);
        }

        private void BecomeTileSelectorPane(Actor sidebarActor, NodePositionAndSize node, Scene scene)
        {
            BecomeBasicPane(sidebarActor);
            var headerSize = 40;
            var horizontalMargin = 16;
            var availableHeight = node.Size.Y - headerSize;
            var availableWidth = node.Size.X - horizontalMargin;
            var rowCount = availableHeight / Grid.TileSize;
            var colCount = availableWidth / Grid.TileSize;

            var rows = new List<LayoutNode>();
            var itemNames = new List<string>();

            rows.Add(LayoutNode.Leaf("header", LayoutSize.StretchedHorizontally(headerSize)));

            for (int i = 0; i < rowCount; i++)
            {
                var rowContent = new List<LayoutNode>();

                rowContent.Add(LayoutNode.StretchedSpacer());


                for (int j = 0; j < colCount; j++)
                {
                    var itemName = $"item {i},{j}";
                    rowContent.Add(LayoutNode.Leaf(itemName, LayoutSize.Pixels(Grid.TileSize, Grid.TileSize)));
                    rowContent.Add(LayoutNode.StretchedSpacer());

                    itemNames.Add(itemName);
                }

                rows.Add(LayoutNode.HorizontalParent($"row{i}", LayoutSize.StretchedHorizontally(Grid.TileSize), LayoutStyle.Empty,
                    rowContent.ToArray()
                ));
                rows.Add(LayoutNode.StretchedSpacer());
            }

            var layout = LayoutNode.VerticalParent("root", LayoutSize.Pixels(node.Size), new LayoutStyle(new Point(16, 0)),
                rows.ToArray()
            );


            var layoutActors = new LayoutActors(scene, layout);
            layoutActors.GetActor("root").transform.SetParent(sidebarActor);

            var templateLibrary = TemplateLibrary.Build();
            var templates = templateLibrary.GetAllTemplates().GetEnumerator();

            foreach (var itemName in itemNames)
            {
                if (!templates.MoveNext())
                {
                    break;
                }

                var template = templates.Current;
                CreateSelectorCell(layoutActors, itemName, template);
            }
        }

        private void CreateSelectorCell(LayoutActors layoutActors, string itemName, IEntityOrTileTemplate template)
        {
            var gridItemActor = layoutActors.GetActor(itemName);
            gridItemActor.GetComponent<BoundingRect>().CenterToBounds();
            new Hoverable(gridItemActor);
            new Clickable(gridItemActor);
            new TemplateSelectorCell(gridItemActor, template, this.templateSelection);

            foreach (var tag in template.Tags)
            {
                if (tag is TileImageTag imageTag)
                {
                    new TileImageRenderer(gridItemActor, imageTag.Image);
                }

                if (tag is SimpleEntityImage entityImage)
                {
                    new EntityImageRenderer(gridItemActor, entityImage.EntityFrameSet);
                }
            }
        }
    }
}
