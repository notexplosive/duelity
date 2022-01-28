using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Duel.Data
{
    public class LevelData : IAsset
    {
        public string Name { get; }
        public TemplateLibrary TemplateLibrary { get; }
        public List<TileInstance> Tiles { get; } = new List<TileInstance>();
        public List<EntityInstance> Entities { get; } = new List<EntityInstance>();
        public List<PropData> Props { get; } = new List<PropData>();

        public LevelData(string levelName)
        {
            Name = levelName;
            TemplateLibrary = TemplateLibrary.Build();
        }

        public static Point ConvertPositionStringToPoint(string xyString)
        {
            var split = xyString.Split(',');
            return new Point(int.Parse(split[0]), int.Parse(split[1]));
        }

        public void LoadForPlay(Level currentLevel, PlayerTag.Type movementType)
        {
            foreach (var tile in Tiles)
            {
                currentLevel.PutTileAt(tile.Position, tile.Template);
            }

            var sheriff = new EntityTemplate(new PlayerTag(PlayerTag.Type.Sheriff));
            var renegade = new EntityTemplate(new PlayerTag(PlayerTag.Type.Renegade));
            var cowboy = new EntityTemplate(new PlayerTag(PlayerTag.Type.Cowboy));
            var knight = new EntityTemplate(new PlayerTag(PlayerTag.Type.Knight));


            foreach (var entity in Entities)
            {
                if (entity.Template.NameInLibrary == "spawn-ernesto")
                {
                    if (movementType == PlayerTag.Type.Sheriff)
                    {
                        currentLevel.PutEntityAt(entity.Position, sheriff);
                        currentLevel.SetCurrentRoomPos(Room.LevelPosToRoomPos(entity.Position));
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-bennigan")
                {
                    if (movementType == PlayerTag.Type.Knight)
                    {
                        currentLevel.PutEntityAt(entity.Position, knight);
                        currentLevel.SetCurrentRoomPos(Room.LevelPosToRoomPos(entity.Position));
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-miranda")
                {
                    if (movementType == PlayerTag.Type.Renegade)
                    {
                        currentLevel.PutEntityAt(entity.Position, renegade);
                        currentLevel.SetCurrentRoomPos(Room.LevelPosToRoomPos(entity.Position));
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-steven")
                {
                    if (movementType == PlayerTag.Type.Cowboy)
                    {
                        currentLevel.PutEntityAt(entity.Position, cowboy);
                        currentLevel.SetCurrentRoomPos(Room.LevelPosToRoomPos(entity.Position));
                    }
                }
                else
                {
                    currentLevel.PutEntityAt(entity.Position, entity.Template);
                }
            }

            foreach (var prop in Props)
            {
                currentLevel.PutPropAt(prop.Position.ToVector2(), prop.Template);
            }
        }

        public void LoadForEditor(Level currentLevel)
        {
            foreach (var tile in Tiles)
            {
                currentLevel.PutTileAt(tile.Position, tile.Template);
            }

            foreach (var entity in Entities)
            {
                currentLevel.PutEntityAt(entity.Position, entity.Template);
            }

            foreach (var prop in Props)
            {
                currentLevel.PutPropAt(prop.Position.ToVector2(), prop.Template);
            }
        }

        public void AddTile(string templateName, Point position)
        {
            Tiles.Add(new TileInstance(position, TemplateLibrary.GetTileTemplate(templateName)));
        }

        public void AddEntity(string templateName, Point position)
        {
            Entities.Add(new EntityInstance(position, TemplateLibrary.GetEntityTemplate(templateName)));
        }

        public void AddProp(string templateName, Point position)
        {
            Props.Add(new PropData(position, TemplateLibrary.GetPropTemplate(templateName)));
        }


        public static LevelData LoadLevelDataFromDisk(string levelName)
        {
            var level = new LevelData(levelName);
            var levelFile = MachinaClient.FileSystem.ReadTextAppDataThenLocal(Path.Join("levels", $"{levelName}.bunk")).Result;
            foreach (var line in levelFile.Split("\n"))
            {
                var tokens = line.Split();
                if (tokens.Length > 1)
                {
                    var templateName = tokens[1].Trim();
                    var position = LevelData.ConvertPositionStringToPoint(tokens[2].Trim());

                    switch (tokens[0])
                    {
                        case "tile":
                            level.AddTile(templateName, position);
                            break;
                        case "entt":
                            level.AddEntity(templateName, position);
                            break;
                        case "prop":
                            level.AddProp(templateName, position);
                            break;
                        default:
                            throw new Exception($"Invalid template class {tokens[0].Trim()}");
                    }
                }
            }

            return level;
        }

        public void OnCleanup()
        {
        }
    }
}