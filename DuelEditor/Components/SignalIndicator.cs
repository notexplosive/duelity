using Duel.Data;
using DuelEditor.Components;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuelEditor.Components
{
    public class SignalIndicator : BaseComponent
    {
        private Level level;

        public SignalIndicator(Actor actor, Level level) : base(actor)
        {
            this.level = level;
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var x = 0;
            foreach (SignalColor color in Enum.GetValues(typeof(SignalColor)))
            {

                var isOn = this.level.SignalState.IsOn(color);
                spriteBatch.DrawCircle(new CircleF(transform.Position + new Vector2(x, 16), 16), 16, SignalToColor(color), isOn ? 16 : 3);

                x += 32;
            }
        }

        private Color SignalToColor(SignalColor color)
        {
            switch (color)
            {
                case SignalColor.Red:
                    return Color.Red;
                case SignalColor.Blue:
                    return Color.Blue;
                case SignalColor.Yellow:
                    return Color.Yellow;
            }

            throw new Exception("rthajrnthjsronth");
        }
    }
}
