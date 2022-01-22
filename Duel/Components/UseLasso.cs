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
    public class UseLasso : BaseComponent
    {
        private readonly ActorRoot actorRoot;
        private readonly Entity userEntity;
        private readonly Level level;
        private readonly KeyboardListener keyboard;
        private readonly LevelSolidProvider solidProvider;

        public UseLasso(Actor actor, Entity entity, Level level, ActorRoot actorRoot) : base(actor)
        {
            this.actorRoot = actorRoot;
            this.userEntity = entity;
            this.level = level;
            this.keyboard = RequireComponent<KeyboardListener>();
            this.solidProvider = new LevelSolidProvider(this.level);
            keyboard.ActionPressed += DeployLasso;
        }

        private void DeployLasso()
        {
            var lasso = CreateProjectile();
            var lassoAnimation = this.actor.scene.StartCoroutine(LassoCoroutine(lasso));
            this.userEntity.BusySignal.Add(new BusyFunction("Lasso", lassoAnimation.IsDone));
        }

        public LassoProjectile CreateProjectile()
        {
            return new LassoProjectile(this.userEntity, this.userEntity.FacingDirection, this.solidProvider);
        }

        public IEnumerator<ICoroutineAction> LassoCoroutine(LassoProjectile lasso)
        {
            if (lasso.Valid)
            {
                yield return lasso.DeployLasso(this.level, actorRoot);

                if (lasso.FoundHook)
                {
                    if (lasso.FoundPullableEntity)
                    {
                        lasso.DestroyLassoActor();
                        yield return new WaitSeconds(0.25f);
                        yield return lasso.PullEntity();
                    }
                    else
                    {
                        yield return new WaitSeconds(0.25f);
                        yield return lasso.JumpToDestination();
                        lasso.DestroyLassoActor();
                    }
                }
                else
                {
                    yield return new WaitSeconds(0.10f);
                    yield return lasso.ReturnLassoToPlayer();
                    lasso.DestroyLassoActor();
                }

            }
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
