using Duel.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class LassoHitScan
    {
        private readonly Entity userEntity;
        private readonly Point startingPosition;
        private readonly Direction throwDirection;
        private readonly Entity entityToPull = null;
        private Entity lassoEntity;
        private readonly bool invalid;
        private Actor actor;
        private Point lassoLandingPosition;

        public bool Valid => !this.invalid;
        public bool FoundHook { get; }
        public bool FoundPullableEntity { get; }

        public LassoHitScan(Entity userEntity, Direction throwDirection, LevelSolidProvider solidProvider)
        {
            this.userEntity = userEntity;
            this.startingPosition = this.userEntity.Position;
            this.throwDirection = throwDirection;
            this.lassoLandingPosition = this.startingPosition;
            FoundHook = false;

            if (solidProvider.HasTagAt<BlockProjectileTag>(this.startingPosition + throwDirection.ToPoint()))
            {
                this.invalid = true;
            }

            bool wasBlocked = false;

            for (int i = 0; i < 3; i++)
            {
                this.lassoLandingPosition += throwDirection.ToPoint();
                var nextPos = this.lassoLandingPosition + throwDirection.ToPoint();

                if (solidProvider.HasTagAt<Grapplable>(this.lassoLandingPosition))
                {
                    FoundHook = true;
                }

                if (solidProvider.HasTagAt<BlockProjectileTag>(nextPos))
                {
                    wasBlocked = true;
                }

                if (FoundHook || wasBlocked)
                {
                    break;
                }
            }

            if (solidProvider.TryGetFirstEntityWithTagAt<Grapplable>(this.lassoLandingPosition, out Entity grapplableEntity))
            {
                if (grapplableEntity.Tags.GetTag<Grapplable>().HookType == Grapplable.Type.PulledByLasso)
                {
                    this.entityToPull = grapplableEntity;
                    FoundPullableEntity = true;
                }
            }

        }

        public ICoroutineAction PullEntity()
        {
            this.entityToPull.JumpToPosition(this.startingPosition + this.throwDirection.ToPoint());
            return new WaitUntil(this.entityToPull.BusySignal.IsFree);
        }

        public ICoroutineAction JumpToDestination()
        {
            this.userEntity.JumpToPosition(this.lassoLandingPosition);
            return new WaitUntil(this.userEntity.BusySignal.GetSpecific("JumpTween").IsFree); // need to probe specific busysignal because lassoing itself raises a busysignal
        }

        public void DestroyLassoActor()
        {
            this.actor.Destroy();
        }

        public ICoroutineAction DeployLasso(Level level, ActorRoot actorRoot)
        {
            this.lassoEntity = level.CreateEntity(this.startingPosition);
            this.lassoEntity.JumpToPosition(this.lassoLandingPosition);
            this.actor = actorRoot.FindActor(this.lassoEntity);
            return new WaitUntil(this.lassoEntity.BusySignal.IsFree);
        }

        public ICoroutineAction ReturnLassoToPlayer()
        {
            this.lassoEntity.JumpToPosition(this.startingPosition);
            return new WaitUntil(this.lassoEntity.BusySignal.IsFree);
        }
    }
}