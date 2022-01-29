using Duel.Components;
using Duel.Data.Dialog;
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
    public class DialogueRunner : BaseComponent
    {
        private float timer;
        private int letterIndex;

        public event Action<IDialogEvent> StartedRun;
        public IDialogEvent PendingEvent { get; private set; }
        public string CurrentText { get; private set; } = "";

        public DialogueRunner(Actor actor) : base(actor)
        {
        }

        public override void Update(float dt)
        {
            this.timer += dt;

            if (this.timer > 0.08f)
            {
                ShowNextLetter();
            }
        }

        private void ShowNextLetter()
        {
            if (PendingEvent is Say say)
            {
                this.letterIndex++;

                if (letterIndex <= say.Text.Length)
                {
                    CurrentText = say.Text.Substring(0, this.letterIndex);
                }
            }
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (PendingEvent is Say say) // <-- YAGNI tax
            {
                if ((key == Keys.Space || key == Keys.Z) && state == ButtonState.Pressed)
                {
                    if (letterIndex > say.Text.Length)
                    {
                        BecomeReady();
                    }
                }
            }
        }

        internal void Run(IDialogEvent pendingAction)
        {
            StartedRun?.Invoke(pendingAction);
            this.letterIndex = 0;
            PendingEvent = pendingAction;
        }

        public bool IsReady()
        {
            return PendingEvent == null;
        }

        public void BecomeReady()
        {
            PendingEvent = null;
        }
    }
}
