using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public struct EditorTileLocation
    {
        public EditorTileLocation(Point position, Rectangle onScreenRect)
        {
            ScreenPositionInEditor = position;
            OnScreenRect = onScreenRect;
        }

        public Point ScreenPositionInEditor { get; }
        public Rectangle OnScreenRect { get; }
        public Point LevelPosition(Point cameraOffset)
        {
            return ScreenPositionInEditor - cameraOffset;
        }

        public Rectangle RenderRect
        {
            get
            {
                var rect = OnScreenRect;
                rect.Inflate(-2, -2);
                return rect;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is EditorTileLocation location &&
                   ScreenPositionInEditor.Equals(location.ScreenPositionInEditor) &&
                   OnScreenRect.Equals(location.OnScreenRect) &&
                   RenderRect.Equals(location.RenderRect);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ScreenPositionInEditor, OnScreenRect, RenderRect);
        }

        public static bool operator ==(EditorTileLocation left, EditorTileLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EditorTileLocation left, EditorTileLocation right)
        {
            return !(left == right);
        }
    }
}
