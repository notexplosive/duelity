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
    public class EditorCore
    {
        private readonly Sokoban game;
        private readonly TemplateSelection templateSelection;
        private TooltipText tooltip;

        public EditorCore(Scene scene, Sokoban game)
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

            BecomeInfoBox(layoutActors.GetActor("bottom-section"), bakedLayout.GetNode("bottom-section"), scene);
            BecomeBasicPane(layoutActors.GetActor("right-sidebar"));

            BecomeTileSelectorPane(layoutActors.GetActor("left-sidebar"), bakedLayout.GetNode("left-sidebar"), scene, this.tooltip);
            BecomeTileEditor(layoutActors.GetActor("tile-editor"), this.tooltip);
        }

        private void BecomeInfoBox(Actor infoBoxActor, NodePositionAndSize node, Scene scene)
        {
            BecomeBasicPane(infoBoxActor);
            var headerSize = 40;

            var layout = LayoutNode.VerticalParent("root", LayoutSize.Pixels(node.Size), LayoutStyle.Empty,
                LayoutNode.Leaf("header", LayoutSize.StretchedHorizontally(headerSize)),
                LayoutNode.HorizontalParent("body", LayoutSize.StretchedBoth(), new LayoutStyle(margin: new Point(15, 5)),
                    LayoutNode.Leaf("text", LayoutSize.StretchedHorizontally(headerSize))
                )
            );

            var layoutActors = new LayoutActors(scene, layout);
            var root = layoutActors.GetActor("root");
            root.transform.SetParent(infoBoxActor);
            root.transform.LocalPosition = Vector2.Zero;
            root.transform.LocalDepth = -10;

            var tooltipActor = layoutActors.GetActor("text");

            new BoundedTextRenderer(tooltipActor, "text goes here", MachinaClient.DefaultStyle.uiElementFont, Color.Black);
            this.tooltip = new TooltipText(tooltipActor);
        }

        private void BecomeTileEditor(Actor actor, TooltipText tooltip)
        {
            game.SetRootActorPosition(actor.transform.Position);
            new Hoverable(actor);
            new RoomEditor(actor, game, this.templateSelection, tooltip);
            new EditorPanner(actor, game);
        }

        private void BecomeBasicPane(Actor sidebarActor)
        {
            new NinepatchRenderer(sidebarActor, MachinaClient.DefaultStyle.windowSheet, NinepatchSheet.GenerationDirection.Inner);
        }

        private void BecomeTileSelectorPane(Actor sidebarActor, NodePositionAndSize node, Scene scene, TooltipText tooltip)
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
            new SignalIndicator(layoutActors.GetActor("header"), this.game.CurrentLevel);

            var templateLibrary = TemplateLibrary.Build();
            var templates = templateLibrary.GetAllTemplates().GetEnumerator();

            foreach (var itemName in itemNames)
            {
                if (!templates.MoveNext())
                {
                    templates.Dispose();
                    break;
                }

                var template = templates.Current;
                CreateSelectorCell(layoutActors, itemName, template, tooltip);
            }

        }

        private void CreateSelectorCell(LayoutActors layoutActors, string itemName, IEntityOrTileTemplate template, TooltipText tooltip)
        {
            var gridItemActor = layoutActors.GetActor(itemName);
            gridItemActor.GetComponent<BoundingRect>().CenterToBounds();
            new Hoverable(gridItemActor);
            new Clickable(gridItemActor);
            new TemplateSelectorCell(gridItemActor, template, this.templateSelection, tooltip);


            foreach (var tag in template.Tags)
            {
                if (tag is TileImageTag imageTag)
                {
                    new TileImageRenderer(gridItemActor, imageTag.Image);
                }
                else if (tag is SimpleEntityImage entityImage)
                {
                    new EntityImageRenderer(gridItemActor, (EntityFrame)entityImage.EntityFrameSet.Normal);
                }
                else if (tag is EditorImage editorImage)
                {
                    new EntityImageRenderer(gridItemActor, editorImage.EntityFrame);
                }
                else if (tag is Key key)
                {
                    new EntityImageRenderer(gridItemActor, (EntityFrame)EntityFrameSet.Key(key.Color).Normal);
                }
                else if (tag is KeyDoor keyDoor)
                {
                    new EntityImageRenderer(gridItemActor, (EntityFrame)EntityFrameSet.KeyDoor(keyDoor.Color).Normal);
                }
                else if (tag is LeverImageTag leverImage)
                {
                    new EntityImageRenderer(gridItemActor, new LeverFrames(leverImage.Color).OffImage);
                }
                else if (tag is PressurePlateImageTag pressurePlateImage)
                {
                    new EntityImageRenderer(gridItemActor, new PressurePlateImages(pressurePlateImage.Color).OffImage);
                }
                else if (tag is SignalDoor signalDoor)
                {
                    var doorImages = signalDoor.DefaultOpened ? (ISignalableImages)new OpenedDoorImages(signalDoor.Color) : new ClosedDoorImages(signalDoor.Color);
                    new EntityImageRenderer(gridItemActor, doorImages.OffImage);
                }
            }
        }
    }
}
