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
        private int blipIndex;

        public event Action<IDialogEvent> StartedRun;
        public IDialogEvent PendingEvent { get; private set; }
        public string CurrentText { get; private set; } = "";

        public DialogueRunner(Actor actor) : base(actor)
        {
        }

        public override void Update(float dt)
        {
            this.timer += dt;

            if (this.timer > 0.02f)
            {
                ShowNextLetter();
                this.timer = 0;
            }
        }

        private void ShowNextLetter()
        {
            if (PendingEvent is Say say)
            {
                this.letterIndex++;

                if (letterIndex <= say.Text.Length)
                {
                    var letter = say.Text[this.letterIndex - 1];
                    if (!char.IsPunctuation(letter) && !char.IsWhiteSpace(letter))
                    {
                        if (this.blipIndex % 2 == 0)
                        {
                            say.Speaker.Blip.Play();
                        }
                        this.blipIndex++;
                    }
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
                    else
                    {
                        letterIndex = say.Text.Length - 1;
                        ShowNextLetter();
                    }
                }
            }
        }

        public void Run(IDialogEvent pendingAction)
        {
            StartedRun?.Invoke(pendingAction);
            this.letterIndex = 0;
            this.blipIndex = 0;
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
