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
        private readonly HashSet<Entity> knownDestroyedActors = new HashSet<Entity>();

        public ActorRoot(Actor actor, Level level) : base(actor)
        {
            this.grid = RequireComponent<Grid>();
            this.level = level;
            this.level.EntityAdded += CreateEntityActor;
            this.level.EntityDestroyRequested += DestroyEntityActor;
        }

        private void DestroyEntityActor(Entity entity, DestroyType type)
        {
            if (!this.knownDestroyedActors.Contains(entity))
            {
                new DestroyWhenBusySignalFree(FindActor(entity), entity.BusySignal, type);
                this.knownDestroyedActors.Add(entity);
            }
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
                if (tag is SimpleEntityImage image)
                {
                    if (!Sokoban.Headless)
                    {
                        new SimpleEntityRenderer(entityActor, image.EntityFrameSet, entity);
                    }
                }
                else if (tag is MiasmaImageTag)
                {
                    if (!Sokoban.Headless)
                    {
                        new MiasmaRenderer(entityActor);
                    }
                }
                else if (tag is Key key)
                {
                    if (!Sokoban.Headless)
                    {
                        new SimpleEntityRenderer(entityActor, EntityFrameSet.Key(key.Color), entity);
                    }
                }
                else if (tag is KeyDoor keyDoor)
                {
                    if (!Sokoban.Headless)
                    {
                        new SimpleEntityRenderer(entityActor, EntityFrameSet.KeyDoor(keyDoor.Color), entity);
                    }
                }
                else if (tag is LeverImageTag leverImage)
                {
                    if (!Sokoban.Headless)
                    {
                        new SignalableRenderer(entityActor, new LeverFrames(leverImage.Color), this.level.SignalState);
                    }
                }
                else if (tag is PressurePlateImageTag pressurePlateImage)
                {
                    if (!Sokoban.Headless)
                    {
                        new SignalableRenderer(entityActor, new PressurePlateImages(pressurePlateImage.Color), this.level.SignalState);
                    }
                }
                else if (tag is SignalDoor signalDoor)
                {
                    if (!Sokoban.Headless)
                    {
                        var doorImages = signalDoor.DefaultOpened ? (ISignalableImages)new OpenedDoorImages(signalDoor.Color) : new ClosedDoorImages(signalDoor.Color);
                        new SignalableRenderer(entityActor, doorImages, this.level.SignalState);
                    }
                }
                else if (tag is PlayerTag playerTag)
                {
                    new LevelTransition(entityActor, this.level, entity);

                    if (playerTag.MovementType == PlayerTag.Type.Sheriff)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new NormalKeyboardMovement(entityActor, entity);
                        new UseLasso(entityActor, entity, this.level, this);

                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, entity, PlayerAnimations.Ernesto);
                            new PlayerDirectionRenderer(entityActor, entity, Color.Crimson);
                            new LassoAnimation(entityActor, entity);
                        }
                    }

                    if (playerTag.MovementType == PlayerTag.Type.Renegade)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new NormalKeyboardMovement(entityActor, entity);
                        new UseGun(entityActor, entity, this.level);

                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, entity, PlayerAnimations.Miranda);
                            new PlayerDirectionRenderer(entityActor, entity, Color.Purple);
                            new GunAnimation(entityActor, entity, this.grid);
                        }
                    }

                    if (playerTag.MovementType == PlayerTag.Type.Cowboy)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new CowboyMovement(entityActor, entity, new LevelSolidProvider(this.level));

                        if (!Sokoban.Headless)
                        {
                            new PlayerCharacterRenderer(entityActor, entity, PlayerAnimations.Steven);
                        }
                    }

                    if (playerTag.MovementType == PlayerTag.Type.Knight)
                    {
                        new BufferedKeyboardListener(entityActor, entity.BusySignal);
                        new KnightMovement(entityActor, entity, new LevelSolidProvider(this.level));
                        new KnightSwing(entityActor, entity);

                        if (!Sokoban.Headless)
                        {
                            new KnightPreviewRenderer(entityActor, entity, this.grid,
                                new LevelSolidProvider(this.level));
                            new KnightCharacterRenderer(entityActor);
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