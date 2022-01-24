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
        private readonly BufferedKeyboardListener keyboard;
        private readonly LevelSolidProvider solidProvider;

        public event Action Deployed;
        public event Action YankStart;
        public event Action JumpStart;
        public event Action Finished;

        public UseLasso(Actor actor, Entity entity, Level level, ActorRoot actorRoot) : base(actor)
        {
            this.actorRoot = actorRoot;
            this.userEntity = entity;
            this.level = level;
            this.solidProvider = new LevelSolidProvider(this.level);

            this.keyboard = RequireComponent<BufferedKeyboardListener>();
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
                Deployed?.Invoke();
                yield return lasso.DeployLasso(this.level, actorRoot);

                if (lasso.FoundGrapplable)
                {
                    lasso.WrapGrappledEntity();
                    lasso.DestroyLassoActor();

                    if (lasso.FoundPullableEntity)
                    {
                        yield return new WaitSeconds(0.25f);
                        this.userEntity.Nudge(this.userEntity.FacingDirection.Opposite);
                        YankStart?.Invoke();
                        yield return lasso.PullEntity();
                    }
                    else
                    {
                        yield return new WaitSeconds(0.25f);
                        JumpStart?.Invoke();
                        yield return lasso.JumpToDestination();
                    }

                    lasso.UnwrapGrappledEntity();
                }
                else
                {
                    if (lasso.WasBlocked)
                    {
                        this.level.NudgeAt(lasso.FailPoint, this.userEntity.FacingDirection);
                        lasso.NudgeLassoEntity(this.userEntity.FacingDirection);
                    }
                    yield return lasso.ReturnLassoToPlayer();
                    lasso.DestroyLassoActor();
                }
            }
            else
            {
                this.userEntity.Nudge(userEntity.FacingDirection.Opposite);
            }

            Finished.Invoke();
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
