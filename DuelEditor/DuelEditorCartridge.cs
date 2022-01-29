using Duel;
using Duel.Data;
using DuelEditor.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;

namespace DuelEditor
{
    public class DuelEditorCartridge : DuelGameCartridge
    {
        public DuelEditorCartridge(Point size) : base(size)
        {
        }

        protected override void LoadGameOrEditor()
        {
            var gameScene = SceneLayers.AddNewScene();
            var game = new Sokoban(gameScene);

            game.StartFresh();
            new EditorCore(SceneLayers.AddNewScene(), game);
        }
    }
}
