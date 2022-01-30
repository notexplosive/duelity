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
            if (state == ButtonState.Pressed && launchCooldown < 0 && this.entity.BusySignal.IsFree())
            {
                if (key == Keys.R)
                {
                    if (modifiers.None)
                    {
                        this.game.RestartRoom();
                    }

                    if (modifiers.Control)
                    {
                        var sceneLayers = this.actor.scene.sceneLayers;
                        sceneLayers.RemoveScene(this.actor.scene);

                        var gameScene = sceneLayers.AddNewScene();
                        DuelGameCartridge.Instance.GetCurrentChapter().Load(gameScene);
                    }
                }
            }
        }
    }
}
