using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public delegate void EntityEvent(Entity entity);

    public class Level
    {
        public event Action TilemapChanged;
        public event EntityEvent EntityAdded;
        public event EntityEvent EntityDestroyRequested;

        private readonly List<Entity> entities = new List<Entity>();

        private readonly Dictionary<Point, TileTemplate> tileMap = new Dictionary<Point, TileTemplate>();

        public SignalState SignalState { get; internal set; } = new SignalState(); // this should be on a per-screen basis

        public Level(Corners corners)
        {
            PutTileAt(corners.TopLeft, new TileTemplate());
            PutTileAt(corners.BottomRight, new TileTemplate());
        }

        public Level() : this(new Corners(Point.Zero, Point.Zero))
        {
        }

        public Entity PutEntityAt(Point startingPosition, EntityTemplate template)
        {
            var entity = template.Create(new LevelSolidProvider(this));

            entity.WarpToPosition(startingPosition);

            AddEntity(entity);

            return entity;
        }

        private void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
            EntityAdded?.Invoke(entity);

            entity.PositionChanged += EntityMoved;
            entity.Bumped += EntityBumped;

            EntityJustSteppedOn(entity.Position);
        }

        private void EntityBumped(Entity entity, Point position, Direction direction)
        {
            if (entity.Tags.TryGetTag(out Key key))
            {
                if (new LevelSolidProvider(this).TryGetFirstEntityWithTagAt(position, out Entity doorEntity, out KeyDoor keyDoor))
                {
                    if (keyDoor.Color == key.Color)
                    {
                        RequestDestroyEntity(doorEntity);
                        RequestDestroyEntity(entity);
                    }
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            this.entities.Remove(entity);

            EntityJustSteppedOff(entity.Position);

            entity.PositionChanged -= EntityMoved;
            entity.Bumped -= EntityBumped;
        }

        public void RequestDestroyEntity(Entity entity)
        {
            EntityDestroyRequested?.Invoke(entity);
        }

        public void PutTileAt(Point position, TileTemplate tile)
        {
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

        private void EntityJustSteppedOn(Point position)
        {
            UpdatePressurePlateAt(position);
        }

        private void UpdatePressurePlateAt(Point position)
        {
            var solidProvider = new LevelSolidProvider(this);
            if (solidProvider.TryGetFirstEntityWithTagAt(position, out Entity foundEntity, out EnableSignalWhenSteppedOn pressurePlateTag))
            {
                if (solidProvider.HasTagAt<Solid>(position) || solidProvider.HasTagAt<PlayerTag>(position))
                {
                    SignalState.TurnOn(pressurePlateTag.Color);
                }
                else
                {
                    SignalState.TurnOff(pressurePlateTag.Color);
                }
            }
        }

        public void EntityJustSteppedOff(Point previousPosition)
        {
            if (GetTileAt(previousPosition).Tags.TryGetTag(out Collapses collapses))
            {
                PutTileAt(previousPosition, collapses.TemplateAfterCollapse);
            }

            UpdatePressurePlateAt(previousPosition);
        }

        private void EntityMoved(Entity mover, MoveType moveType, Point previousPosition)
        {
            if (moveType != MoveType.Warp)
            {
                EntityJustSteppedOff(previousPosition);
                EntityJustSteppedOn(mover.Position);
            }
        }
    }
}