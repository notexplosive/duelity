using Duel.Components;
using Duel.Data;
using DuelEditor.Components;
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
        private EditorTileLocation? hoveredTile;
        private bool leftMouseDown;
        public Point CameraOffset { get; set; }

        public TileEditor(Actor actor, Level level) : base(actor)
        {
            this.level = level;
        }

        public override void Update(float dt)
        {
            if (this.hoveredTile.HasValue)
            {
                if (this.leftMouseDown)
                {
                    this.level.PutTileAt(this.hoveredTile.Value.LevelPosition(CameraOffset), new TileTemplate(new TileImageTag(TileImageTag.TileImage.Wall)));
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

                level.GetTileAt(tile.LevelPosition(CameraOffset));
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
