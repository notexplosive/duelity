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
    public class LevelTransition : BaseComponent
    {
        private Level level;
        private Entity playerEntity;

        public LevelTransition(Actor actor, Level level, Entity playerEntity) : base(actor)
        {
            this.level = level;
            this.playerEntity = playerEntity;
        }

        public override void Update(float dt)
        {
            MachinaClient.Print(this.playerEntity.Position, this.level.LevelPosToRoomPos(this.playerEntity.Position));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
