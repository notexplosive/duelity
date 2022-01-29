using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public delegate void EntityEvent(Entity entity);
    public delegate void DestroyEvent(Entity entity, DestroyType destroyType);

    public class Level
    {
        public event Action TilemapChanged;
        public event Action<Vector2, PropTemplate> PropAdded;
        public event Action GoToNextLevel;
        public event Action<string> ConversationStarted;
        public event EntityEvent EntityAdded;
        public event DestroyEvent EntityDestroyRequested;
        public event Action RemoveAllProps;
        public event Action<Point, Point> RoomTransitionAttempted;
        public event Action<Entity, Entity, Point> RoomTransitionFinished;

        private readonly List<Entity> entities = new List<Entity>();

        private readonly Dictionary<Point, TileTemplate> tileMap = new Dictionary<Point, TileTemplate>();
        private readonly SignalState signalOverrideLayer = new SignalState();

        public SignalState SignalState { get; internal set; } = new SignalState(); // this should be on a per-screen basis
        public SignalState LeversState { get; internal set; } = new SignalState();

        public Level(Corners corners)
        {
            PutTileAt(corners.TopLeft, new TileTemplate());
            PutTileAt(corners.BottomRight, new TileTemplate());
        }

        public void ClearAllTilesAndEntities()
        {
            this.tileMap.Clear();

            foreach (var entity in this.entities)
            {
                RequestDestroyEntity(entity, DestroyType.Vanish);
            }
            this.entities.Clear();

            RemoveAllProps?.Invoke();
        }

        public Level() : this(new Corners(Point.Zero, Point.Zero))
        {
        }

        public Entity PutEntityAt(Point startingPosition, EntityTemplate template)
        {
            var entity = template.Create(new LevelSolidProvider(this));

            entity.WarpToPosition(startingPosition);

            AddEntity(entity);
            UpdateSignalState();

            return entity;
        }

        public void PutPropAt(Vector2 worldPosition, PropTemplate template)
        {
            PropAdded?.Invoke(worldPosition, template);
        }

        private void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
            EntityAdded?.Invoke(entity);

            entity.PositionChanged += EntityMoved;
            entity.Bumped += EntityBumped;

            EntityJustSteppedOn(entity, entity.Position);
        }

        public IEnumerable<TileInstance> AllKnownTiles()
        {
            foreach (var tile in this.tileMap)
            {
                yield return new TileInstance(tile.Key, tile.Value);
            }
        }

        private void EntityBumped(Entity entity, Point position, Direction direction)
        {
            if (entity.Tags.TryGetTag(out Key key))
            {
                if (new LevelSolidProvider(this).TryGetFirstEntityWithTagAt(position, out Entity doorEntity, out KeyDoor keyDoor))
                {
                    if (keyDoor.Color == key.Color)
                    {
                        RequestDestroyEntity(doorEntity, DestroyType.Vanish);
                        RequestDestroyEntity(entity, DestroyType.Break);
                    }
                }
            }
        }

        public void OnPlayerRoomTransition(Entity playerFromPreviousRoom, Entity playerFromThisRoom, PlayerTag.Type moveType, Point playerPosition)
        {
            RoomTransitionFinished?.Invoke(playerFromPreviousRoom, playerFromThisRoom, playerPosition);
            LeversState = new SignalState();
            UpdateSignalState();
        }

        public void ClearTileAt(Point position)
        {
            this.tileMap.Remove(position);
            TilemapChanged?.Invoke();
        }

        public void RemoveEntity(Entity entity)
        {
            this.entities.Remove(entity);

            EntityJustSteppedOff(entity.Position);

            entity.PositionChanged -= EntityMoved;
            entity.Bumped -= EntityBumped;

            UpdateSignalState();
        }

        public void RequestDestroyEntity(Entity entity, DestroyType destroyType)
        {
            EntityDestroyRequested?.Invoke(entity, destroyType);
        }

        public void PutTileAt(Point position, TileTemplate tile)
        {
            if (tile.NameInLibrary == "")
            {
                tile.NameInLibrary = "emptyTile";
            }

            this.tileMap[position] = tile;
            TilemapChanged?.Invoke();
        }

        public AutoTile ComputeAutoTile(params TileImageTag.TileImage[] images)
        {
            var result = new AutoTile();
            foreach (var tileAndLocation in this.tileMap)
            {
                if (tileAndLocation.Value.Tags.TryGetTag(out TileImageTag tileImageTag))
                {
                    foreach (var image in images)
                    {
                        if (tileImageTag.Image == image)
                        {
                            result.PutTileAt(tileAndLocation.Key);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public Corners CalculateCorners()
        {
            var minTile = Point.Zero;
            var maxTile = Point.Zero;
            foreach (var tilePosition in tileMap.Keys)
            {
                minTile = new Point(Math.Min(minTile.X, tilePosition.X), Math.Min(minTile.Y, tilePosition.Y));
                maxTile = new Point(Math.Max(maxTile.X, tilePosition.X), Math.Max(maxTile.Y, tilePosition.Y));
            }

            return new Corners(minTile, maxTile);
        }

        public void ForceSignalOn(SignalColor color)
        {
            this.signalOverrideLayer.TurnOn(color);
        }

        public TileTemplate GetTileAt(Point position)
        {
            this.tileMap.TryGetValue(position, out TileTemplate result);
            if (result == null)
            {
                return new TileTemplate();
            }

            return result;
        }

        public void NudgeAt(Point point, Direction direction)
        {
            foreach (var entity in AllEntitiesAt(point))
            {
                entity.Nudge(direction);
            }
        }

        public Entity[] AllEntities()
        {
            return this.entities.ToArray();
        }

        public bool IsOutOfBounds(Point position)
        {
            var corners = CalculateCorners();
            return position.X < corners.TopLeft.X || position.Y < corners.TopLeft.Y ||
                   position.X >= corners.BottomRight.X || position.Y >= corners.BottomRight.Y;
        }

        public IEnumerable<Entity> AllEntitiesAt(Point position)
        {
            var entitiesCopy = new List<Entity>(this.entities);
            foreach (var entity in entitiesCopy)
            {
                if (entity.Position == position)
                {
                    yield return entity;
                }
            }
        }

        private void EntityJustSteppedOn(Entity stepper, Point position)
        {
            FillWaterIfApplicable(stepper, position);

            TransitionLevelIfApplicable(stepper, position);

            TalkToNpcIfApplicable(stepper, position);
        }

        private void TalkToNpcIfApplicable(Entity stepper, Point position)
        {
            foreach (var entity in AllEntities())
            {
                if (entity.Tags.TryGetTag(out NpcTag npcTag))
                {
                    if (Math.Abs(entity.Position.X - position.X) <= 1 && Math.Abs(entity.Position.Y - position.Y) <= 1) // is within one square
                    {
                        if (stepper.Tags.TryGetTag(out PlayerTag player))
                        {
                            var key = npcTag.GetConversationKey(player.MovementType);
                            if (!DuelGameCartridge.Instance.Screenplay.HasConversationAlreadyHappened(key))
                            {
                                ConversationStarted?.Invoke(key);
                            }
                        }
                    }
                }
            }
        }

        private void TransitionLevelIfApplicable(Entity stepper, Point position)
        {
            var solidProvider = new LevelSolidProvider(this);
            if (solidProvider.HasTagAt<ZoneTransitionTrigger>(position) && stepper.Tags.HasTag<PlayerTag>())
            {
                GoToNextLevel?.Invoke();
            }
        }

        private void ChangeRoomsIfApplicable(Entity stepper, Point previousPosition, Point position)
        {
            if (stepper.Tags.HasTag<PlayerTag>())
            {
                RoomTransitionAttempted?.Invoke(previousPosition, position);
            }
        }

        private void FillWaterIfApplicable(Entity stepper, Point position)
        {
            var solidProvider = new LevelSolidProvider(this);
            var waterTileExists = solidProvider.HasTagAt<UnfilledWater>(position);
            var ravineTileExists = solidProvider.HasTagAt<Ravine>(position);

            if (stepper.Tags.TryGetTag(out WaterFiller waterFiller))
            {
                if (ravineTileExists)
                {
                    RequestDestroyEntity(stepper, DestroyType.Fall);
                }

                if (waterTileExists)
                {
                    if (waterFiller.FillerType == WaterFiller.Type.Floats)
                    {
                        var tags = new TagCollection();
                        foreach (var tag in GetTileAt(position).Tags)
                        {
                            if (!(tag is UnfilledWater))
                            {
                                tags.AddTag(tag);
                            }
                        }

                        tags.AddTag(new FilledWater(stepper));
                        PutTileAt(position, new TileTemplate(tags));
                    }

                    RequestDestroyEntity(stepper, DestroyType.Sink);
                }
            }
        }

        public void EntityJustSteppedOff(Point previousPosition)
        {
            if (GetTileAt(previousPosition).Tags.TryGetTag(out Collapses collapses))
            {
                PutTileAt(previousPosition, collapses.TemplateAfterCollapse);
            }
        }

        private void EntityMoved(Entity mover, MoveType moveType, Point previousPosition)
        {
            if (moveType != MoveType.Warp)
            {
                EntityJustSteppedOff(previousPosition);
                EntityJustSteppedOn(mover, mover.Position);
                ChangeRoomsIfApplicable(mover, previousPosition, mover.Position);
            }

            UpdateSignalState();
        }

        public void UpdateSignalState()
        {
            var solidProvider = new LevelSolidProvider(this);
            var pressurePlateIsDown = new HashSet<SignalColor>();
            var leverIsOn = new HashSet<SignalColor>();

            foreach (var entity in this.entities)
            {
                var position = entity.Position;
                if (entity.Tags.TryGetTag(out EnableSignalWhenSteppedOn pressurePlateTag))
                {
                    if (solidProvider.HasTagAt<Solid>(position) || solidProvider.HasTagAt<PlayerTag>(position))
                    {
                        pressurePlateIsDown.Add(pressurePlateTag.Color);
                    }
                }

                if (entity.Tags.TryGetTag(out ToggleSignal toggleSignal))
                {
                    if (LeversState.IsOn(toggleSignal.Color))
                    {
                        leverIsOn.Add(toggleSignal.Color);
                    }
                }
            }

            foreach (SignalColor color in Enum.GetValues(typeof(SignalColor)))
            {
                if (this.signalOverrideLayer.IsOn(color))
                {
                    SignalState.TurnOn(color);
                    continue;
                }

                if (pressurePlateIsDown.Contains(color) || leverIsOn.Contains(color))
                {
                    SignalState.TurnOn(color);
                }
                else
                {
                    SignalState.TurnOff(color);
                }
            }
        }
    }
}