using Machina.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Duel.Data
{
    public class PropTemplate : ITemplate
    {
        public PropTemplate(string imageName)
        {
            Texture = MachinaClient.Assets.GetTexture("props_large_cactus");
        }

        public Texture2D Texture { get; }

        public TagCollection Tags => new TagCollection();
    }
}