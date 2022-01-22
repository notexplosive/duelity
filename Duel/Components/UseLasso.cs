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
            var lassoAnimation = this.actor.scene.StartCoroutine(LassoCoroutine(this.userEntity.FacingDirection));
            this.userEntity.BusySignal.Add(new BusyFunction("Lasso", lassoAnimation.IsDone));
        }

        private IEnumerator<ICoroutineAction> LassoCoroutine(Direction throwDirection)
        {
            var hitScan = new LassoHitScan(this.userEntity, throwDirection, this.solidProvider);
            if (hitScan.Valid)
            {
                yield return hitScan.DeployLasso(this.level, actorRoot);

                if (hitScan.FoundHook)
                {
                    if (hitScan.FoundPullableEntity)
                    {
                        hitScan.DestroyLassoActor();
                        yield return new WaitSeconds(0.25f);
                        yield return hitScan.PullEntity();
                    }
                    else
                    {
                        yield return new WaitSeconds(0.25f);
                        yield return hitScan.JumpToDestination();
                        hitScan.DestroyLassoActor();
                    }
                }
                else
                {
                    yield return new WaitSeconds(0.10f);
                    yield return hitScan.ReturnLassoToPlayer();
                    hitScan.DestroyLassoActor();
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
