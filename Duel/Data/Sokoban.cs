using Duel.Components;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Duel.Data
{
    public class Sokoban
    {
        public event Action<Room> RoomChanged;
        private Actor rootActor;
        private Tuple<LevelData, PlayerTag.Type> previouslyLoadedData;
        public Point? SavedPlayerPosition { get; private set; }

        public Point? CurrentRoomPos { get; set; } = null;
        public Scene Scene { get; }
        public Grid Grid { get; private set; }

        public ActorRoot ActorRoot { get; private set; }

        public Level CurrentLevel { get; private set; }

        // Set to true for tests (ughhhhhhhhhhhhhhhhh)
        public static bool Headless { get; set; }
        public TileGridRenderer TileRenderer { get; private set; }
        public static IEnumerable<string> LevelNames
        {
            get
            {
                yield return "level_1";
                yield return "level_2";
                yield return "level_3";
                yield return "level_4";
                yield return "level_5";
                yield return "level_6";
                yield return "level_7";
                yield return "level_8";
                yield return "level_9";
            }
        }

        public void RestartRoom()
        {
            ReloadLevelAndPutPlayerAtPosition(SavedPlayerPosition.Value, SavedPlayerPosition.Value);
        }

        public Sokoban(Scene scene)
        {
            Scene = scene;
            StartFresh();
        }

        public void PlayLevel(LevelData levelData, PlayerTag.Type playerCharacter)
        {
            CurrentLevel.ClearAllTilesAndEntities();
            levelData.LoadAndActivatePlayerSpawners(this, playerCharacter);

            // We get the loaded level's data minus the player
            var dataMinusPlayer = BuildDataFromCurrentLevel("live-from-gameplay");
            var player = dataMinusPlayer.Entities.Find(e => e.Template.Tags.HasTag<PlayerTag>());
            dataMinusPlayer.Entities.Remove(player);

            this.previouslyLoadedData = new Tuple<LevelData, PlayerTag.Type>(dataMinusPlayer, playerCharacter);
        }

        public void LoadLevelData(LevelData levelData)
        {
            CurrentLevel.ClearAllTilesAndEntities();
            levelData.LoadForEditor(CurrentLevel);
        }

        public void ReloadLevelAndPutPlayerAtPosition(Point playerPreviousPosition, Point playerPosition)
        {
            SavedPlayerPosition = playerPosition;

            if (this.previouslyLoadedData != null)
            {
                Entity previousPlayer = null;
                foreach (var entity in CurrentLevel.AllEntities())
                {
                    if (entity.Tags.HasTag<PlayerTag>())
                    {
                        previousPlayer = entity;
                    }
                }

                CurrentLevel.ClearAllTilesAndEntities();

                // There won't be any player spawners to activate
                this.previouslyLoadedData.Item1.LoadAndActivatePlayerSpawners(this, this.previouslyLoadedData.Item2);

                var playerTemplate = TemplateLibrary.GetPlayerTemplate(this.previouslyLoadedData.Item2);
                var player = CurrentLevel.PutEntityAt(playerPreviousPosition, playerTemplate);

                if (playerPreviousPosition != playerPosition)
                {
                    // only do this if this is a room transition and not a reset
                    CurrentLevel.OnPlayerRoomTransition(previousPlayer, player, this.previouslyLoadedData.Item2, playerPosition);
                }
            }

            foreach (var entity in CurrentLevel.AllEntities())
            {
                if (CurrentRoomPos != Room.LevelPosToRoomPos(entity.Position))
                {
                    CurrentLevel.RequestDestroyEntity(entity, DestroyType.Vanish);
                }
            }
        }

        private void DoRoomTransitionIfApplicable(Point previousPlayerPosition, Point playerPosition)
        {
            var newRoom = new Room(Room.LevelPosToRoomPos(playerPosition));
            if (!CurrentRoomPos.HasValue)
            {
                EnterRoom(newRoom.Position, previousPlayerPosition, playerPosition);
                return;
            }

            var previousRoom = new Room(CurrentRoomPos.Value);

            if (previousRoom != newRoom)
            {
                EnterRoom(newRoom.Position, previousPlayerPosition, playerPosition);
            }
        }

        public void EnterRoom(Point roomPos, Point previousPosition, Point newPosition)
        {

            if (CurrentRoomPos != roomPos)
            {
                SavedPlayerPosition = newPosition;
                CurrentRoomPos = roomPos;
                RoomChanged?.Invoke(new Room(roomPos));
                ReloadLevelAndPutPlayerAtPosition(previousPosition, newPosition);
            }
        }

        public void StartFresh()
        {
            this.rootActor?.Delete();

            CurrentLevel = new Level();
            CurrentLevel.RoomTransitionAttempted += DoRoomTransitionIfApplicable;

            this.rootActor = Scene.AddActor("Level");

            Grid = new Grid(this.rootActor, CurrentLevel);
            ActorRoot = new ActorRoot(this.rootActor, this);

            if (!Headless)
            {
                TileRenderer = new TileGridRenderer(this.rootActor, CurrentLevel);
            }
        }

        public Actor FindActor(Entity entity)
        {
            return ActorRoot.FindActor(entity);
        }

        public LevelData BuildDataFromCurrentLevel(string name)
        {
            var data = new LevelData(name);
            foreach (var instance in ActorRoot.GetAllInstances())
            {
                switch (instance.TemplateClassName)
                {
                    case "entt":
                        data.AddEntity(instance.TemplateName, LevelData.ConvertPositionStringToPoint(instance.CoordinateString));
                        break;
                    case "tile":
                        data.AddTile(instance.TemplateName, LevelData.ConvertPositionStringToPoint(instance.CoordinateString));
                        break;
                    case "prop":
                        data.AddProp(instance.TemplateName, LevelData.ConvertPositionStringToPoint(instance.CoordinateString));
                        break;
                    default:
                        MachinaClient.Print($"skipped {instance.TemplateClassName} {instance.TemplateName}");
                        break;
                }
            }
            return data;
        }

        public void SetRootActorPosition(Vector2 position)
        {
            ActorRoot.transform.Position = position;
        }

        public Vector2 GetRootActorPosition()
        {
            return ActorRoot.transform.Position;
        }
    }
}
