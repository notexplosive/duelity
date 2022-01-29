using Machina.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Duel.Data
{
    public class PropTemplate : ITemplate
    {
        public enum PropLayeringRule
        {
            Behind,
            Front
        }

        public PropTemplate(string imageName, PropLayeringRule layeringRule)
        {
            Texture = MachinaClient.Assets.GetTexture(imageName);
            LayeringRule = layeringRule;
        }

        public Texture2D Texture { get; }
        public PropLayeringRule LayeringRule { get; }

        public TagCollection Tags => new TagCollection();

        public string NameInLibrary { get; set; }
    }
}