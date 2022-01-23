using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class LassoAnimation : BaseComponent
    {
        private readonly PlayerCharacterRenderer renderer;
        private readonly UseLasso useLasso;
        private readonly Entity entity;

        public LassoAnimation(Actor actor, Entity entity) : base(actor)
        {
            this.renderer = RequireComponent<PlayerCharacterRenderer>();
            this.useLasso = RequireComponent<UseLasso>();
            this.entity = entity;


            this.useLasso.Deployed += NudgeForward;
            this.useLasso.Deployed += OverrideAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-throw"));
            this.useLasso.YankStart += OverrideAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-jump"));
            this.useLasso.JumpStart += OverrideAnimation(MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>("ernesto-jump"));
            this.useLasso.Finished += ClearOverride;
        }

        private void NudgeForward()
        {
            this.entity.Nudge(entity.FacingDirection.Opposite);
        }

        private void ClearOverride()
        {
            this.renderer.AnimationOverride = AnimationWrapper.None;
        }

        private Action OverrideAnimation(IFrameAnimation frameAnimation)
        {
            return () => this.renderer.AnimationOverride = new AnimationWrapper(frameAnimation);
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
