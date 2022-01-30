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
    public class KnightPreviewRenderer : BaseComponent
    {
        private readonly KnightMovement movement;
        private readonly Entity entity;
        private readonly Grid grid;
        private readonly SolidProvider solidProvider;

        public KnightPreviewRenderer(Actor actor, Entity entity, Grid grid, SolidProvider solidProvider) : base(actor)
        {
            this.movement = RequireComponent<KnightMovement>();
            this.entity = entity;
            this.grid = grid;
            this.solidProvider = solidProvider;
        }

        public override void Update(float dt)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var currentRoom = new Room(Room.LevelPosToRoomPos(entity.Position));
            var cameraNormalPos = this.grid.TileToLocalPosition(currentRoom.GetBounds().TopLeft);
            var cameraAdjustedPos = cameraNormalPos;

            if (this.movement.LongLeg != Direction.None)
            {
                var longLeg = this.movement.LongLeg.ToPoint() + this.movement.LongLeg.ToPoint();
                var offset1 = this.movement.LongLeg.Next.ToPoint();
                var offset2 = this.movement.LongLeg.Previous.ToPoint();
                var possibleLandingSpots = new Point[] {
                    this.entity.Position + longLeg + offset1,
                    this.entity.Position + longLeg + offset2,
                    this.entity.Position
                };

                foreach (var spot in possibleLandingSpots)
                {
                    var radius = Grid.TileSize / 4;
                    var isSolid = this.solidProvider.IsNotWalkableAt(this.entity, spot);
                    spriteBatch.DrawCircle(new CircleF(this.grid.TileToLocalPosition(spot), radius), 25, isSolid ? Color.OrangeRed : Color.White, isSolid ? radius / 4 : radius / 2, transform.Depth - 10);
                }

                var difference = Vector2.Zero;

                foreach (var possibleLandingSpot in possibleLandingSpots)
                {
                    var worldPos = this.grid.TileToLocalPosition(possibleLandingSpot, true) + this.grid.transform.Position;
                    var viewportRect = new RectangleF(cameraNormalPos, this.actor.scene.camera.UnscaledViewportSize);
                    if (!viewportRect.Contains(worldPos))
                    {
                        if (worldPos.Y > viewportRect.Bottom)
                        {
                            difference = new Vector2(0, viewportRect.Bottom - worldPos.Y);
                        }

                        if (worldPos.Y < viewportRect.Top)
                        {
                            difference = new Vector2(0, viewportRect.Top - worldPos.Y + Grid.TileSize / 2); // just for the top
                        }

                        if (worldPos.X < viewportRect.Left)
                        {
                            difference = new Vector2(viewportRect.Left - worldPos.X, 0);
                        }

                        if (worldPos.X > viewportRect.Right)
                        {
                            difference = new Vector2(viewportRect.Right - worldPos.X, 0);
                        }
                    }
                }

                cameraAdjustedPos -= difference;
            }

            this.actor.scene.camera.UnscaledPosition = cameraAdjustedPos;
        }
    }
}
