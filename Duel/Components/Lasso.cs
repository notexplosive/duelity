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
    public class Lasso : BaseComponent
    {
        private readonly KeyboardListener keyboard;

        public Lasso(Actor actor, Entity entity) : base(actor)
        {
            this.keyboard = RequireComponent<KeyboardListener>();
            keyboard.ActionPressed += DeployLasso;
        }

        private void DeployLasso()
        {
            MachinaClient.Print("Lasso deployed");
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
