using Duel.Components;
using Machina.Data;
using Machina.Engine;
using Machina.ThirdParty;
using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class LassoProjectile
    {
        private readonly Entity userEntity;
        private readonly Point startingPosition;
        private readonly Entity entityToPull = null;
        private Entity lassoEntity;
        private readonly bool invalid;
        private Actor actor;

        public Direction ThrowDirection { get; }
        public Point LassoLandingPosition { get; }
        public bool Valid => !this.invalid;
        public bool FoundGrapplable { get; }
        public bool FoundPullableEntity { get; }
        public bool WasBlocked { get; }
        public Point FailPoint { get; }
        public bool IsReturning { get; private set; }
        public Entity RopedEntity { get; private set; }
        public Point? RopedLocation { get; private set; }

        public LassoProjectile(Entity userEntity, Direction throwDirection, LevelSolidProvider solidProvider)
        {
            this.userEntity = userEntity;
            this.startingPosition = this.userEntity.Position;
            this.ThrowDirection = throwDirection;
            LassoLandingPosition = this.startingPosition;
            FoundGrapplable = false;

            if (solidProvider.BlocksBulletsAt(this.startingPosition, this.startingPosition + throwDirection.ToPoint()))
            {
                this.invalid = true;
            }

            if (Valid)
            {
                WasBlocked = false;

                for (int i = 0; i < 3; i++)
                {
                    LassoLandingPosition += throwDirection.ToPoint();
                    var nextPos = LassoLandingPosition + throwDirection.ToPoint();

                    if (solidProvider.HasTagAt<Grapplable>(LassoLandingPosition))
                    {
                        FoundGrapplable = true;
                    }

                    if (solidProvider.BlocksBulletsAt(this.userEntity.Position, LassoLandingPosition))
                    {
                        WasBlocked = true;
                    }

                    if (WasBlocked && !FoundGrapplable)
                    {
                        FailPoint = LassoLandingPosition;
                        LassoLandingPosition -= throwDirection.ToPoint();
                    }

                    if (FoundGrapplable || WasBlocked)
                    {
                        break;
                    }
                }

                if (solidProvider.TryGetFirstEntityWithTagAt(LassoLandingPosition, out Entity grapplableEntity, out Grapplable grapplable))
                {
                    if (grapplable.HookType == Grapplable.Type.PulledByLasso)
                    {
                        this.entityToPull = grapplableEntity;
                        FoundPullableEntity = true;
                    }
                }
            }
        }

        public void NudgeLassoEntity(Direction direction)
        {
            this.lassoEntity.Nudge(direction);
        }

        public ICoroutineAction PullEntity()
        {
            this.entityToPull.JumpToPosition(this.startingPosition + this.ThrowDirection.ToPoint());
            return new WaitUntil(this.entityToPull.BusySignal.IsFree);
        }

        public ICoroutineAction JumpToDestination()
        {
            this.userEntity.JumpToPosition(LassoLandingPosition);
            return new WaitUntil(this.userEntity.BusySignal.GetSpecific("JumpTween").IsFree); // need to probe specific busysignal because lassoing itself raises a busysignal
        }

        public void DestroyLassoActor()
        {
            this.actor.Destroy();
        }

        public ICoroutineAction DeployLasso(Level level, ActorRoot actorRoot)
        {
            this.lassoEntity = level.PutEntityAt(this.startingPosition, new EntityTemplate());
            this.lassoEntity.JumpToPosition(LassoLandingPosition, EaseFuncs.QuadraticEaseOut);
            this.actor = actorRoot.FindActor(this.lassoEntity);
            if (!Sokoban.Headless)
            {
                new LassoRenderer(this.actor, this, actorRoot.FindActor(userEntity), actorRoot);
            }
            return new WaitUntil(this.lassoEntity.BusySignal.IsFree);
        }

        public ICoroutineAction ReturnLassoToPlayer()
        {
            this.lassoEntity.JumpToPosition(this.startingPosition, EaseFuncs.QuadraticEaseOut);
            IsReturning = true;
            return new WaitUntil(this.lassoEntity.BusySignal.IsFree);
        }

        public void WrapGrappledEntity()
        {
            if (FoundPullableEntity)
            {
                this.entityToPull.GrabWithLasso();
                RopedEntity = this.entityToPull;
            }
            RopedLocation = LassoLandingPosition;
        }

        public void UnwrapGrappledEntity()
        {
            if (FoundPullableEntity)
            {
                this.entityToPull.ReleaseFromLasso();
            }
        }
    }
}