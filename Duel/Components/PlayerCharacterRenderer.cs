﻿using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class PlayerCharacterRenderer : BaseComponent
    {
        private readonly EntityRenderInfo renderInfo;
        private readonly Actor spriteActor;
        private readonly SpriteRenderer spriteRenderer;

        public PlayerCharacterRenderer(Actor actor) : base(actor)
        {
            this.renderInfo = RequireComponent<EntityRenderInfo>();
            this.spriteActor = transform.AddActorAsChild("sprite");
            this.spriteRenderer = new SpriteRenderer(this.spriteActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet"));

            this.spriteRenderer.SetAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-idle"));
            this.spriteRenderer.FramesPerSecond = 10;
            spriteActor.transform.LocalPosition = new Vector2(0, -Grid.TileSize / 4);
            this.renderInfo.DisableDebugGraphic();
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            // technically we don't need to do this, but if we're in framestep it looks weird if we don't
            BindSpritePosToRenderOffset();
        }

        public override void Update(float dt)
        {
            BindSpritePosToRenderOffset();
            if (RenderOffset().Length() > 5)
            {
                this.spriteRenderer.SetAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-move"));
            }
            else
            {
                this.spriteRenderer.SetAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-idle"));
            }
        }

        private void BindSpritePosToRenderOffset()
        {
            this.spriteRenderer.SetOffset(RenderOffset());
        }

        private Vector2 RenderOffset()
        {
            return -this.renderInfo.renderOffsetTweenable.getter();
        }
    }
}