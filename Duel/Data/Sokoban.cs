﻿using Duel.Components;
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
        private Actor rootActor;

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
            }
        }

        public Sokoban(Scene scene)
        {
            Scene = scene;
            StartFresh();
        }

        public void LoadLevel(LevelData levelData)
        {
            CurrentLevel.ClearAllTilesAndEntities();
            levelData.Load(CurrentLevel);
        }

        public void StartFresh()
        {
            this.rootActor?.Delete();

            CurrentLevel = new Level();
            this.rootActor = Scene.AddActor("Level");

            Grid = new Grid(this.rootActor, CurrentLevel);
            ActorRoot = new ActorRoot(this.rootActor, CurrentLevel);

            if (!Headless)
            {
                TileRenderer = new TileGridRenderer(this.rootActor, CurrentLevel);
            }
        }

        public Actor FindActor(Entity entity)
        {
            return ActorRoot.FindActor(entity);
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
