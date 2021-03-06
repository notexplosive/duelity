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
            TemplateLibrary = TemplateLibrary.BuildWithPlayers();
        }

        public static Point ConvertPositionStringToPoint(string xyString)
        {
            var split = xyString.Split(',');
            return new Point(int.Parse(split[0]), int.Parse(split[1]));
        }

        public Point LoadAndGetSpawnPosition(Sokoban game, PlayerTag.Type movementType)
        {
            var currentLevel = game.CurrentLevel;
            foreach (var tile in Tiles)
            {
                currentLevel.PutTileAt(tile.Position, tile.Template);
            }


            var positionToEnter = Point.Zero;
            foreach (var entity in Entities)
            {
                var position = entity.Position;

                if (entity.Template.NameInLibrary == "spawn-ernesto")
                {
                    if (movementType == PlayerTag.Type.Sheriff)
                    {
                        currentLevel.PutEntityAt(position, TemplateLibrary.GetEntityTemplate("sheriff"));
                        positionToEnter = position;
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-bennigan")
                {
                    if (movementType == PlayerTag.Type.Knight)
                    {
                        currentLevel.PutEntityAt(position, TemplateLibrary.GetEntityTemplate("knight"));
                        positionToEnter = position;
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-miranda")
                {
                    if (movementType == PlayerTag.Type.Renegade)
                    {
                        currentLevel.PutEntityAt(position, TemplateLibrary.GetEntityTemplate("renegade"));
                        positionToEnter = position;
                    }
                }
                else if (entity.Template.NameInLibrary == "spawn-steven")
                {
                    if (movementType == PlayerTag.Type.Cowboy)
                    {
                        currentLevel.PutEntityAt(position, TemplateLibrary.GetEntityTemplate("cowboy"));
                        positionToEnter = position;
                    }
                }
                else
                {
                    currentLevel.PutEntityAt(position, entity.Template);
                }
            }

            foreach (var prop in Props)
            {
                currentLevel.PutPropAt(prop.Position.ToVector2(), prop.Template);
            }

            return positionToEnter;
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