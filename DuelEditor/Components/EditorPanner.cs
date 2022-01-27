using Duel.Components;
using Duel.Data;
using DuelEditor.Components;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Components
{
    public class EditorPanner : BaseComponent
    {
        private readonly Sokoban game;
        private readonly RoomEditor tileEditor;

        public Point CameraPosition { get; private set; }

        public EditorPanner(Actor actor, Sokoban game) : base(actor)
        {
            this.game = game;
            this.tileEditor = RequireComponent<RoomEditor>();
        }

        public override void Update(float dt)
        {
            game.SetRootActorPosition(transform.Position + CameraPosition.ToVector2() * Grid.TileSize);
            this.tileEditor.CameraOffset = CameraPosition;
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (state == ButtonState.Pressed)
            {
                int x = 1;
                if (modifiers.Shift)
                {
                    x *= 10;
                }

                if (key == Keys.A)
                {
                    CameraPosition += new Point(x, 0);
                }

                if (key == Keys.D)
                {
                    CameraPosition += new Point(-x, 0);
                }

                if (key == Keys.W)
                {
                    CameraPosition += new Point(0, x);
                }

                if (key == Keys.S)
                {
                    CameraPosition += new Point(0, -x);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
