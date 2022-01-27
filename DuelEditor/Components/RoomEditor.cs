﻿using Duel.Components;
using Duel.Data;
using DuelEditor.Components;
using DuelEditor.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Machina.Engine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Components
{
    public class RoomEditor : BaseComponent
    {
        private readonly Hoverable hoverable;
        private readonly Level level;
        private readonly TemplateSelection templateSelection;
        private readonly Sokoban game;
        private readonly TooltipText tooltip;
        private bool leftMouseDown;
        private bool rightMouseDown;
        private bool hasPlacedEntity;

        public EditorTileLocation? HoveredTile { get; private set; }
        public Point CameraOffset { get; set; }

        public RoomEditor(Actor actor, Sokoban game, TemplateSelection templateSelection, TooltipText tooltipText) : base(actor)
        {
            this.hoverable = RequireComponent<Hoverable>();
            this.level = game.CurrentLevel;
            this.templateSelection = templateSelection;
            this.game = game;
            this.tooltip = tooltipText;
        }

        public override void Update(float dt)
        {
            if (this.hoverable.IsHovered && this.HoveredTile.HasValue)
            {
                var position = this.HoveredTile.Value.LevelPosition(CameraOffset);
                this.tooltip.Add($"world coords: {position.X}, {position.Y}");
                var room = new Room(Room.LevelPosToRoomPos(position));
                this.tooltip.Add($"room pos: {room.Position.X}, {room.Position.Y}");
                if (this.leftMouseDown)
                {
                    if (this.templateSelection.Primary is TileTemplate tileTemplate)
                    {
                        this.level.PutTileAt(position, tileTemplate);
                    }

                    if (!this.hasPlacedEntity)
                    {
                        this.hasPlacedEntity = true;
                        if (this.templateSelection.Primary is EntityTemplate entityTemplate)
                        {
                            this.level.PutEntityAt(position, entityTemplate);
                        }
                    }
                }

                if (this.rightMouseDown)
                {
                    if (this.templateSelection.IsInEntityMode)
                    {
                        ClearEntitiesAt(position, DestroyType.Break);
                    }

                    if (this.templateSelection.IsInTileMode)
                    {
                        this.level.ClearTileAt(position);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in GetEditorTileLocations())
            {
                var position = tile.LevelPosition(CameraOffset);

                if (position == new Point(-Room.Size.X, -Room.Size.Y))
                {
                    var realRectPosition = game.GetRootActorPosition() + game.Grid.TileToLocalPosition(position, false);
                    spriteBatch.DrawRectangle(new RectangleF(realRectPosition, new Point(10)), Color.White, 5f, transform.Depth);
                }

                var room = new Room(Room.LevelPosToRoomPos(position));
                var bounds = room.GetBounds();
                if (bounds.TopLeft == position)
                {
                    var realRectSize = (Size2)Room.Size * Grid.TileSize;
                    var realRectPosition = game.GetRootActorPosition() + game.Grid.TileToLocalPosition(position, false);
                    var realRect = new RectangleF(realRectPosition, realRectSize);
                    spriteBatch.DrawRectangle(realRect, Color.White, 5f, transform.Depth);
                }
            }
        }

        public override void OnMouseUpdate(Vector2 currentPosition, Vector2 positionDelta, Vector2 rawDelta)
        {
            this.HoveredTile = null;

            foreach (var tile in GetEditorTileLocations())
            {
                if (tile.OnScreenRect.Contains(currentPosition))
                {
                    this.HoveredTile = tile;
                }
            }
        }

        public override void OnMouseButton(MouseButton button, Vector2 currentPosition, ButtonState state)
        {
            if (button == MouseButton.Left)
            {
                this.leftMouseDown = state == ButtonState.Pressed;

                if (state == ButtonState.Released)
                {
                    this.hasPlacedEntity = false;
                }
            }

            if (button == MouseButton.Right)
            {
                this.rightMouseDown = state == ButtonState.Pressed;
            }
        }

        public IEnumerable<EditorTileLocation> GetEditorTileLocations()
        {
            for (int y = 0; y < Room.Size.Y + 2; y++)
            {
                for (int x = 0; x < Room.Size.X + 2; x++)
                {
                    yield return new EditorTileLocation(new Point(x, y), new Rectangle(new Point(x * Grid.TileSize, y * Grid.TileSize) + transform.Position.ToPoint(), new Point(Grid.TileSize)));
                }
            }
        }

        private void ClearEntitiesAt(Point position, DestroyType destroyType)
        {
            foreach (var entity in this.level.AllEntitiesAt(position))
            {
                this.level.RequestDestroyEntity(entity, destroyType);
            }
        }
    }

}
