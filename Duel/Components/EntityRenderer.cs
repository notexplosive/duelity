using Duel.Data;
using Machina.Components;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Duel.Components
{
    public class EntityRenderer : BaseComponent
    {
        private readonly LevelRenderer levelRenderer;
        private readonly Entity entity;

        public EntityRenderer(Actor actor, LevelRenderer levelRenderer, Entity entity) : base(actor)
        {
            this.levelRenderer = levelRenderer;
            this.entity = entity;
            SnapPositionToGrid();
        }

        public void SnapPositionToGrid()
        {
            transform.LocalPosition = this.levelRenderer.TileToLocalPosition(this.entity.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(new CircleF(transform.Position, 20), 20, Color.Orange, 20, transform.Depth);
        }
    }
}
