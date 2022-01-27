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
    public class TileEditor : BaseComponent
    {
        private readonly Level level;
        private readonly TemplateSelection templateSelection;
        private EditorTileLocation? hoveredTile;
        private bool leftMouseDown;
        private bool rightMouseDown;

        public Point CameraOffset { get; set; }

        public TileEditor(Actor actor, Level level, TemplateSelection templateSelection) : base(actor)
        {
            this.level = level;
            this.templateSelection = templateSelection;
        }

        public override void Update(float dt)
        {
            if (this.hoveredTile.HasValue)
            {
                var position = this.hoveredTile.Value.LevelPosition(CameraOffset);
                if (this.leftMouseDown)
                {
                    if (this.templateSelection.Primary is TileTemplate tileTemplate)
                    {
                        this.level.PutTileAt(position, tileTemplate);
                    }

                    if (this.templateSelection.Primary is EntityTemplate entityTemplate)
                    {
                        this.level.PutEntityAt(position, entityTemplate);
                    }
                }

                if (this.rightMouseDown)
                {
                    if (this.templateSelection.IsInEntityMode)
                    {
                        foreach (var entity in this.level.AllEntitiesAt(position))
                        {
                            this.level.RequestDestroyEntity(entity, DestroyType.Break);
                        }
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
                var isHovered = tile == this.hoveredTile;
                var unHoveredColor = Color.LightBlue;

                var roomPos = Room.LevelPosToRoomPos(tile.LevelPosition(CameraOffset));

                if ((roomPos.X % 2 == 0 && roomPos.Y % 2 == 1) || (roomPos.X % 2 == 1 && roomPos.Y % 2 == 0))
                {
                    unHoveredColor = Color.Orange;
                }

                spriteBatch.DrawRectangle(tile.RenderRect, isHovered ? Color.White : unHoveredColor, isHovered ? 3f : 1f, transform.Depth - 10);

                this.level.GetTileAt(tile.LevelPosition(CameraOffset));
            }
        }

        public override void OnMouseUpdate(Vector2 currentPosition, Vector2 positionDelta, Vector2 rawDelta)
        {
            this.hoveredTile = null;

            foreach (var tile in GetEditorTileLocations())
            {
                if (tile.OnScreenRect.Contains(currentPosition))
                {
                    this.hoveredTile = tile;
                }
            }
        }

        public override void OnMouseButton(MouseButton button, Vector2 currentPosition, ButtonState state)
        {
            if (button == MouseButton.Left)
            {
                this.leftMouseDown = state == ButtonState.Pressed;
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
    }

}
