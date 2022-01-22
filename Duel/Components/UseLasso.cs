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
            var targetPosition = this.userEntity.Position;

            if (this.solidProvider.HasTagAt<BlockProjectileTag>(targetPosition + throwDirection.ToPoint()))
            {

            }
            else
            {
                bool foundHook = false;
                bool wasBlocked = false;

                for (int i = 0; i < 3; i++)
                {
                    targetPosition += throwDirection.ToPoint();
                    var nextPos = targetPosition + throwDirection.ToPoint();

                    if (this.solidProvider.HasTagAt<Grapplable>(targetPosition))
                    {
                        foundHook = true;
                    }

                    if (this.solidProvider.HasTagAt<BlockProjectileTag>(nextPos))
                    {
                        wasBlocked = true;
                    }

                    if (foundHook || wasBlocked)
                    {
                        break;
                    }
                }

                // Actually do the animation
                var lassoEntity = this.level.CreateEntity(this.userEntity.Position);

                lassoEntity.JumpToPosition(targetPosition);
                yield return new WaitUntil(lassoEntity.BusySignal.IsFree);

                if (foundHook)
                {
                    if (this.solidProvider.TryGetFirstEntityWithTagAt<Grapplable>(targetPosition, out Entity grapplableEntity))
                    {
                        if (grapplableEntity.Tags.GetTag<Grapplable>().HookType == Grapplable.Type.PulledByLasso)
                        {
                            this.actorRoot.FindActor(lassoEntity).Destroy();
                            yield return new WaitSeconds(0.25f);
                            grapplableEntity.JumpToPosition(this.userEntity.Position + throwDirection.ToPoint());
                            yield return new WaitUntil(grapplableEntity.BusySignal.IsFree);
                        }
                    }
                    else
                    {
                        yield return new WaitSeconds(0.25f);
                        this.userEntity.JumpToPosition(targetPosition);
                        yield return new WaitUntil(this.userEntity.BusySignal.GetSpecific("JumpTween").IsFree); // need to probe specific busysignal because lassoing itself raises a busysignal
                        this.actorRoot.FindActor(lassoEntity).Destroy();
                    }
                }
                else
                {
                    yield return new WaitSeconds(0.10f);
                    lassoEntity.JumpToPosition(this.userEntity.Position);
                    yield return new WaitUntil(lassoEntity.BusySignal.IsFree);
                    this.actorRoot.FindActor(lassoEntity).Destroy();
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
