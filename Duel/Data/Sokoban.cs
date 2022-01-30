using Duel.Components;
using Duel.Data.Dialog;
using Machina.Data;
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

        private TemplateLibrary templateLibraryWithPlayers;

        public Scene Scene { get; }
        public Grid Grid { get; private set; }

        public ActorRoot ActorRoot { get; private set; }

        public Level CurrentLevel { get; private set; }

        // Set to true for tests (ughhhhhhhhhhhhhhhhh)
        public static bool Headless { get; set; }
        public TileGridRenderer TileRenderer { get; private set; }
        public static IEnumerable<string> LevelContentNames
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

        public Sokoban(Scene scene, ZoneTileset zoneTileset = ZoneTileset.Thistown)
        {
            this.templateLibraryWithPlayers = TemplateLibrary.BuildWithPlayers();
            Scene = scene;
            StartFresh(zoneTileset);
        }

        public void PlayLevel(LevelData levelData, PlayerTag.Type playerCharacter)
        {
            CurrentLevel.ClearAllTilesAndEntities();
            var spawnPosition = levelData.LoadAndGetSpawnPosition(this, playerCharacter);

            // We get the loaded level's data minus the player
            var dataMinusPlayer = BuildDataFromCurrentLevel("live-from-gameplay");
            var player = dataMinusPlayer.Entities.Find(e => e.Template.Tags.HasTag<PlayerTag>());
            dataMinusPlayer.Entities.Remove(player);

            this.previouslyLoadedData = new Tuple<LevelData, PlayerTag.Type>(dataMinusPlayer, playerCharacter);

            EnterRoom(Room.LevelPosToRoomPos(spawnPosition), spawnPosition, spawnPosition);
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
                this.previouslyLoadedData.Item1.LoadAndGetSpawnPosition(this, this.previouslyLoadedData.Item2);

                var playerTemplate = this.templateLibraryWithPlayers.GetPlayerTemplateForMoveType(this.previouslyLoadedData.Item2);
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

        public void StartDialogue(Conversation conversation)
        {
            var coroutine = Scene.StartCoroutine(ShowDialogueConversation(conversation));

            var busyFunction = new BusyFunction("Dialogue", coroutine.IsDone);

            foreach (var entity in CurrentLevel.AllEntities())
            {
                if (entity.Tags.HasTag<PlayerTag>())
                {
                    entity.BusySignal.Add(busyFunction);
                }
            }
        }

        private IEnumerator<ICoroutineAction> ShowDialogueConversation(Conversation conversation)
        {
            // create dialogue box (layout etc)
            var actor = Scene.AddActor("Dialogue");
            var dialogueRunner = new DialogueRunner(actor);
            new DialogueBoxRenderer(actor);

            foreach (var conversationEvent in conversation.Events)
            {
                if (conversationEvent is Say say)
                {
                    dialogueRunner.Run(say);
                    yield return new WaitUntil(dialogueRunner.IsReady);
                }
            }

            actor.Destroy();
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

        public void StartFresh(ZoneTileset zoneTileset)
        {
            this.rootActor?.Delete();

            CurrentLevel = new Level();
            CurrentLevel.ZoneTileset = zoneTileset;
            CurrentLevel.ConversationStarted += RunConversationFromScreenplay;
            CurrentLevel.RoomTransitionAttempted += DoRoomTransitionIfApplicable;
            CurrentLevel.GoToNextLevel += AdvanceToNextLevel;

            this.rootActor = Scene.AddActor("Level");

            Grid = new Grid(this.rootActor, CurrentLevel);
            ActorRoot = new ActorRoot(this.rootActor, this);

            if (!Headless)
            {
                TileRenderer = new TileGridRenderer(this.rootActor, CurrentLevel);
            }
        }

        private void AdvanceToNextLevel()
        {
            CurrentLevel.GoToNextLevel -= AdvanceToNextLevel;
            CurrentLevel.ConversationStarted -= RunConversationFromScreenplay;
            CurrentLevel.RoomTransitionAttempted -= DoRoomTransitionIfApplicable;

            var sceneLayers = Scene.sceneLayers;
            var overlay = sceneLayers.AddNewScene();
            new LevelTransitionOverlayAnimation(overlay.AddActor("LevelTransitionActor"), Scene);


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

        public void RunConversationFromScreenplay(string key)
        {
            StartDialogue(DuelGameCartridge.Instance.Screenplay.GetConversation(key));
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
