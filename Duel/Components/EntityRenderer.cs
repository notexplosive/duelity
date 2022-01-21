﻿using Duel.Data;
using Machina.Components;
using Machina.Engine;

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
    }
}
