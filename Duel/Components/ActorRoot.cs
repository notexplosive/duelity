﻿using Duel.Components;
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
    public delegate void EntityActorAction(Actor actor, Entity entity);

    public class ActorRoot : BaseComponent
    {
        public event EntityActorAction EntityActorSpawned;

        private readonly Level level;
        private readonly Dictionary<Entity, Actor> entityToActorTable = new Dictionary<Entity, Actor>();

        public ActorRoot(Actor actor, Level level) : base(actor)
        {
            this.level = level;
            this.level.EntityAdded += CreateEntityActor;
        }

        private void CreateEntityActor(Entity entity)
        {
            var entityActor = transform.AddActorAsChild("EntityActor");
            entityActor.transform.LocalDepth -= 200;
            entityToActorTable[entity] = entityActor;

            new RemoveEntityOnDestroy(entityActor, this.level, entity);
            EntityActorSpawned?.Invoke(entityActor, entity);
        }
    }
}
