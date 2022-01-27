using Duel.Components;
using Machina.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class Sokoban
    {
        private readonly Scene scene;
        private Actor rootActor;

        public Grid Grid { get; private set; }

        private ActorRoot actorRootComponent;

        public Level CurrentLevel { get; private set; }

        // Set to true for tests (ughhhhhhhhhhhhhhhhh)
        public static bool Headless { get; set; }

        public Sokoban(Scene scene)
        {
            this.scene = scene;
            ClearEverything();
        }

        public void ClearEverything()
        {
            this.rootActor?.Delete();

            CurrentLevel = new Level();
            this.rootActor = scene.AddActor("Level");

            Grid = new Grid(this.rootActor, CurrentLevel);
            this.actorRootComponent = new ActorRoot(this.rootActor, CurrentLevel);

            if (!Headless)
            {
                new TileGridRenderer(this.rootActor, CurrentLevel);
            }
        }

        public Actor FindActor(Entity entity)
        {
            return this.actorRootComponent.FindActor(entity);
        }

        public void SetRootActorPosition(Vector2 position)
        {
            this.actorRootComponent.transform.Position = position;
        }

        public Vector2 GetRootActorPosition()
        {
            return this.actorRootComponent.transform.Position;
        }
    }
}
