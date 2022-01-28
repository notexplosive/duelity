using Machina.Engine;
using Microsoft.Xna.Framework;
using System;

namespace Duel.Data
{
    public class PropInstance : TemplateInstance
    {
        private readonly Actor actor;

        public PropInstance(Actor actor, PropTemplate propTemplate)
        {
            Template = propTemplate;
            this.actor = actor;
        }

        public PropTemplate Template { get; }


        public Vector2 Position => actor.transform.Position;
        public bool IsDestroyed => actor.IsDestroyed;

        public override string TemplateName => Template.NameInLibrary;

        public override string CoordinateString
        {
            get
            {
                var pos = this.actor.transform.LocalPosition;
                pos.Floor();
                return $"{pos.X},{pos.Y}";
            }
        }

        protected override TemplateClass TemplateClass => TemplateClass.Prop;

        public void Destroy()
        {
            this.actor.Destroy();
        }
    }
}