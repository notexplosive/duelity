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
    public delegate void EntityActorAction(Actor actor, Entity entity);

    public class ActorRoot : BaseComponent
    {
        private readonly Grid grid;
        private readonly Level level;
        private readonly Dictionary<Entity, Actor> entityToActorTable = new Dictionary<Entity, Actor>();

        public ActorRoot(Actor actor, Level level) : base(actor)
        {
            this.grid = RequireComponent<Grid>();
            this.level = level;
            this.level.EntityAdded += CreateEntityActor;
            this.level.EntityDestroyRequested += DestroyEntityActor;
        }

        private void DestroyEntityActor(Entity entity)
        {
            FindActor(entity).Destroy();
        }

        private void CreateEntityActor(Entity entity)
        {
            var entityActor = transform.AddActorAsChild("EntityActor");
            entityActor.transform.LocalDepth -= 200;
            entityToActorTable[entity] = entityActor;

            new RemoveEntityOnDestroy(entityActor, this.level, entity);
            new EntityRenderInfo(entityActor, this.grid, entity);
            new MovementRenderer(entityActor, entity);

            ApplyTags(entity, entityActor);
        }

        private void ApplyTags(Entity entity, Actor entityActor)
        {
            foreach (var tag in entity.Tags)
            {
                if (tag is PlayerTag playerTag)
                {
                    if (playerTag.MovementType == PlayerTag.Type.Sheriff)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new NormalKeyboardMovement(entityActor, entity);
                        new UseLasso(entityActor, entity, this.level, this);

                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, PlayerAnimations.Ernesto);
                        }
                    }

                    if (playerTag.MovementType == PlayerTag.Type.Renegade)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new NormalKeyboardMovement(entityActor, entity);
                        new UseGun(entityActor, entity, this.level);
                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, PlayerAnimations.Miranda);
                        }
                    }

                    if (playerTag.MovementType == PlayerTag.Type.Cowboy)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new ChargingKeyboardMovement(entityActor, entity, new LevelSolidProvider(this.level));

                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, PlayerAnimations.Steven);
                        }
                    }
                }
            }
        }

        public Actor FindActor(Entity entity)
        {
            return this.entityToActorTable[entity];
        }
    }
}
