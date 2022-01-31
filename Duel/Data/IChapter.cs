using Machina.Engine;

namespace Duel.Data
{
    public interface IChapter
    {
        PlayerTag.Type Player { get; }
        void Load(Scene gameScene);
    }
}
