﻿using Duel.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class LassoProjectile
    {
        private readonly Entity userEntity;
        private readonly Point startingPosition;
        private readonly Direction throwDirection;
        private readonly Entity entityToPull = null;
        private Entity lassoEntity;
        private readonly bool invalid;
        private Actor actor;

        public Point LassoLandingPosition { get; }
        public bool Valid => !this.invalid;
        public bool FoundHook { get; }
        public bool FoundPullableEntity { get; }

        public LassoProjectile(Entity userEntity, Direction throwDirection, LevelSolidProvider solidProvider)
        {
            this.userEntity = userEntity;
            this.startingPosition = this.userEntity.Position;
            this.throwDirection = throwDirection;
            LassoLandingPosition = this.startingPosition;
            FoundHook = false;

            if (solidProvider.HasTagAt<BlockProjectileTag>(this.startingPosition + throwDirection.ToPoint()))
            {
                this.invalid = true;
            }

            if (Valid)
            {
                bool wasBlocked = false;

                for (int i = 0; i < 3; i++)
                {
                    LassoLandingPosition += throwDirection.ToPoint();
                    var nextPos = LassoLandingPosition + throwDirection.ToPoint();

                    if (solidProvider.HasTagAt<Grapplable>(LassoLandingPosition))
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

                if (solidProvider.TryGetFirstEntityWithTagAt<Grapplable>(LassoLandingPosition, out Entity grapplableEntity))
                {
                    if (grapplableEntity.Tags.GetTag<Grapplable>().HookType == Grapplable.Type.PulledByLasso)
                    {
                        this.entityToPull = grapplableEntity;
                        FoundPullableEntity = true;
                    }
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
            this.userEntity.JumpToPosition(LassoLandingPosition);
            return new WaitUntil(this.userEntity.BusySignal.GetSpecific("JumpTween").IsFree); // need to probe specific busysignal because lassoing itself raises a busysignal
        }

        public void DestroyLassoActor()
        {
            this.actor.Destroy();
        }

        public ICoroutineAction DeployLasso(Level level, ActorRoot actorRoot)
        {
            this.lassoEntity = level.CreateEntity(this.startingPosition);
            this.lassoEntity.JumpToPosition(LassoLandingPosition);
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