using Machina.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace Duel.Data
{
    public class PropTemplate : ITemplate
    {
        public PropTemplate(string imageName)
        {
            Texture = MachinaClient.Assets.GetTexture(imageName);
        }

        public Texture2D Texture { get; }

        public TagCollection Tags => new TagCollection();

        public string NameInLibrary { get; set; }
    }
}