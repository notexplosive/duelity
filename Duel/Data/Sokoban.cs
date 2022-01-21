using Duel.Components;
using Machina.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class Sokoban
    {
        private readonly Scene scene;
        private Actor rootActor;

        public Level CurrentLevel { get; private set; }

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
            this.rootActor.transform.Depth -= 200;

            new ActorRoot(this.rootActor, CurrentLevel);
            new LevelRenderer(this.rootActor, CurrentLevel);
        }
    }
}
