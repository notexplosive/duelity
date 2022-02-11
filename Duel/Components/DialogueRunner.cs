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
        public int LetterIndex { get; private set; }
        private int blipIndex;

        public event Action<IDialogEvent> StartedRun;
        public IDialogEvent PendingEvent { get; private set; }

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
                LetterIndex++;

                if (LetterIndex <= say.GetFormattedTextNoFont().TotalCharacterCount)
                {
                    var letter = say.GetFormattedTextNoFont().GetCharacterAt(LetterIndex - 1);
                    if (!char.IsPunctuation(letter) && !char.IsWhiteSpace(letter))
                    {
                        if (this.blipIndex % 2 == 0)
                        {
                            say.Speaker.Blip.Play();
                        }
                        this.blipIndex++;
                    }
                }
            }
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (PendingEvent is Say say) // <-- YAGNI tax
            {
                if ((key == Keys.Space || key == Keys.Z) && state == ButtonState.Pressed)
                {
                    if (LetterIndex > say.GetFormattedTextNoFont().TotalCharacterCount)
                    {
                        BecomeReady();
                    }
                    else
                    {
                        LetterIndex = say.GetFormattedTextNoFont().TotalCharacterCount - 1;
                        ShowNextLetter();
                    }
                }
            }
        }

        public void Run(IDialogEvent pendingAction)
        {
            StartedRun?.Invoke(pendingAction);
            LetterIndex = 0;
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
