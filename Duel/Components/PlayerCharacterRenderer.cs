using Duel.Components;
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
        private readonly Entity entity;
        private readonly PlayerAnimations playerAnimations;
        private readonly MovementRenderer movement;
        private readonly EntityRenderInfo renderInfo;
        private readonly Actor spriteActor;
        private readonly SpriteRenderer spriteRenderer;

        public PlayerCharacterRenderer(Actor actor, Entity entity, PlayerAnimations playerAnimations) : base(actor)
        {
            this.entity = entity;
            this.playerAnimations = playerAnimations;
            this.movement = RequireComponent<MovementRenderer>();
            this.renderInfo = RequireComponent<EntityRenderInfo>();

            this.spriteActor = transform.AddActorAsChild("sprite");
            this.spriteRenderer = new SpriteRenderer(this.spriteActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("characters-sheet"));

            this.spriteRenderer.SetAnimation(playerAnimations.Idle);
            this.spriteRenderer.FramesPerSecond = 10;
            spriteActor.transform.LocalPosition = new Vector2(0, -Grid.TileSize / 4);
            this.renderInfo.DisableDebugGraphic();
        }

        public AnimationWrapper AnimationOverride { get; set; } = AnimationWrapper.None;

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            // technically we don't need to do this, but if we're in framestep it looks weird if we don't
            BindSpritePosToRenderOffset();
        }

        public override void Update(float dt)
        {
            BindSpritePosToRenderOffset();

            if (AnimationOverride == AnimationWrapper.None)
            {
                if (this.movement.IsMoving)
                {
                    this.spriteRenderer.SetAnimation(this.playerAnimations.Move);
                }
                else
                {
                    this.spriteRenderer.SetAnimation(this.playerAnimations.Idle);
                }
            }
            else
            {
                this.spriteRenderer.SetAnimation(AnimationOverride.Animation);
            }

            if (this.entity.FacingDirection == Direction.Left)
            {
                this.spriteRenderer.FlipX = false;
            }

            if (this.entity.FacingDirection == Direction.Right)
            {
                this.spriteRenderer.FlipX = true;
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
