using Duel.Components;
using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Components
{
    public class RestartRoomButton : BaseComponent
    {
        private float launchCooldown;
        private readonly Entity entity;
        private readonly Sokoban game;

        public RestartRoomButton(Actor actor, Entity entity, Sokoban game) : base(actor)
        {
            this.launchCooldown = 0.25f;
            this.entity = entity;
            this.game = game;
        }

        public override void Update(float dt)
        {
            this.launchCooldown -= dt;
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (key == Keys.R && modifiers.None && state == ButtonState.Pressed && launchCooldown < 0 && this.entity.BusySignal.IsFree())
            {
                this.game.ReloadLevelAndPutPlayerAtPosition(this.game.SavedPlayerPosition.Value, this.game.SavedPlayerPosition.Value);
            }
        }
    }
}
