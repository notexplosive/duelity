using Duel.Components;
using Duel.Data.Dialog;
using Machina.Data;
using Machina.Engine;
using System;
using System.Collections.Generic;

namespace Duel.Data
{
    public class Dialogue
    {
        public event Action<string> InvokeHappened;

        public Scene Scene { get; }

        private readonly Level level;

        public Dialogue(Scene scene, Level level)
        {
            Scene = scene;
            this.level = level;
        }


        public WaitUntil StartDialogue(Conversation conversation)
        {
            var coroutine = Scene.StartCoroutine(ShowDialogueConversation(conversation));

            var busyFunction = new BusyFunction("Dialogue", coroutine.IsDone);

            foreach (var entity in level.AllEntities())
            {
                if (entity.Tags.HasTag<PlayerTag>())
                {
                    entity.BusySignal.Add(busyFunction);
                }
            }



            return coroutine;
        }

        private IEnumerator<ICoroutineAction> ShowDialogueConversation(Conversation conversation)
        {
            // create dialogue box (layout etc)
            var actor = Scene.AddActor("Dialogue");
            var dialogueRunner = new DialogueRunner(actor);
            new DialogueBoxRenderer(actor);

            foreach (var conversationEvent in conversation.Events)
            {
                if (conversationEvent is Invoke invoke)
                {
                    InvokeHappened?.Invoke(invoke.EventName);
                }

                if (conversationEvent is Say say)
                {
                    dialogueRunner.Run(say);
                }
                yield return new WaitUntil(dialogueRunner.IsReady);
            }

            actor.Destroy();
        }
    }
}
