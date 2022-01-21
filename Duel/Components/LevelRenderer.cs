using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class LevelRenderer : BaseComponent
    {
        public static readonly int TileSize = 60;

        private readonly Level level;
        private Tuple<Point, Point> levelCorners;

        public LevelRenderer(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            ContentChanged();

            this.level.ContentChanged += ContentChanged;
        }

        private void ContentChanged()
        {
            this.levelCorners = this.level.CalculateCorners();
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = this.levelCorners.Item1.X; x < this.levelCorners.Item2.X; x++)
            {
                for (int y = this.levelCorners.Item1.Y; y < this.levelCorners.Item2.Y; y++)
                {
                    DrawCell(spriteBatch, new Point(x, y));
                }
            }
        }

        public void DrawCell(SpriteBatch spriteBatch, Point location)
        {
            spriteBatch.DrawRectangle(new RectangleF(TileToLocalPosition(location), new Point(TileSize)), Color.White, 1f, transform.Depth);
            spriteBatch.DrawCircle(new CircleF(location.ToVector2() * TileSize, TileSize / 2), 25, Color.White, 1f, transform.Depth);
        }

        public Vector2 TileToLocalPosition(Point location, bool centered = true)
        {
            return location.ToVector2() * TileSize - (centered ? new Vector2(TileSize) / 2 : Vector2.Zero);
        }
    }
}
