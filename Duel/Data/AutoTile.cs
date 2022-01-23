using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public enum TileClass
    {
        None,
        VerticalNub,
        HorizontalNub,
        CenterNub,
        BottomNub,
        LeftNub,
        RightNub,
        TopNub,
        Alone,
        CenterFill,
        BottomEdge,
        LeftEdge,
        RightEdge,
        TopEdge,
        TopRightCorner,
        TopLeftCorner,
        BottomLeftCorner,
        BottomRightCorner
    }

    public class AutoTile
    {
        private readonly HashSet<Point> map;

        public AutoTile()
        {
            this.map = new HashSet<Point>();
        }

        public void PutTileAt(Point point)
        {
            this.map.Add(point);
        }

        public bool ExistsAt(Point point)
        {
            return this.map.Contains(point);
        }

        public TileClass GetClassAt(Point point)
        {
            if (!ExistsAt(point))
            {
                return TileClass.None;
            }

            var existsLeft = ExistsAt(point + Direction.Left.ToPoint());
            var existsRight = ExistsAt(point + Direction.Right.ToPoint());
            var existsDown = ExistsAt(point + Direction.Down.ToPoint());
            var existsUp = ExistsAt(point + Direction.Up.ToPoint());
            var existsBottomLeft = ExistsAt(point + Direction.Left.ToPoint() + Direction.Down.ToPoint());
            var existsBottomRight = ExistsAt(point + Direction.Right.ToPoint() + Direction.Down.ToPoint());
            var existsTopRight = ExistsAt(point + Direction.Right.ToPoint() + Direction.Up.ToPoint());
            var existsTopLeft = ExistsAt(point + Direction.Left.ToPoint() + Direction.Up.ToPoint());

            if (existsLeft && existsRight && existsDown && existsUp)
            {
                if (existsBottomLeft || existsBottomRight || existsTopRight || existsTopLeft)
                {
                    return TileClass.CenterFill;
                }

                return TileClass.CenterNub;
            }

            if (existsLeft && existsDown && !existsRight && !existsUp)
            {
                return TileClass.TopRightCorner;
            }

            if (existsRight && existsDown && !existsLeft && !existsUp)
            {
                return TileClass.TopLeftCorner;
            }

            if (existsLeft && existsUp && !existsRight && !existsDown)
            {
                return TileClass.BottomRightCorner;
            }

            if (existsRight && existsUp && !existsLeft && !existsDown)
            {
                return TileClass.BottomLeftCorner;
            }

            if (existsLeft && existsRight)
            {
                if (existsUp && !existsDown)
                {
                    return TileClass.BottomEdge;
                }

                if (!existsUp && existsDown)
                {
                    return TileClass.TopEdge;
                }

                return TileClass.HorizontalNub;
            }

            if (existsUp && existsDown)
            {
                if (existsLeft && !existsRight)
                {
                    return TileClass.RightEdge;
                }

                if (!existsLeft && existsRight)
                {
                    return TileClass.LeftEdge;
                }

                return TileClass.VerticalNub;
            }

            if (existsUp)
            {
                return TileClass.BottomNub;
            }

            if (existsDown)
            {
                return TileClass.TopNub;
            }

            if (existsRight)
            {
                return TileClass.LeftNub;
            }

            if (existsLeft)
            {
                return TileClass.RightNub;
            }

            return TileClass.Alone;
        }
    }
}
