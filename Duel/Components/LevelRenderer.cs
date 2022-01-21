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
        public static readonly int TileSize = 64;

        private readonly Level level;
        private Tuple<Point, Point> levelCorners;

        public LevelRenderer(Actor actor, Level level) : base(actor)
        {
            RequireComponent<ActorRoot>().EntityActorSpawned += SetupActorRenderer;

            this.level = level;
            RecomputeCorners();

            this.level.TilemapChanged += RecomputeCorners;
        }

        private void SetupActorRenderer(Actor entityActor, Entity entity)
        {
            new EntityRenderer(entityActor, this, entity);
        }

        private void RecomputeCorners()
        {
            this.levelCorners = this.level.CalculateCorners();
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
            var tileDepth = transform.Depth + 100;
            spriteBatch.DrawRectangle(new RectangleF(TileToLocalPosition(location, false), new Point(TileSize)), Color.White, 1f, tileDepth);
            spriteBatch.DrawCircle(new CircleF(TileToLocalPosition(location, true), TileSize / 2), 25, Color.White, 1f, tileDepth);
        }

        public Vector2 TileToLocalPosition(Point location, bool centered = true)
        {
            return location.ToVector2() * TileSize + (centered ? new Vector2(TileSize) / 2 : Vector2.Zero);
        }
    }
}
