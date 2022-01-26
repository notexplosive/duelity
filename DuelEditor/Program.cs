using Machina.Data;
using Machina.Engine;
using MachinaDesktop;
using Microsoft.Xna.Framework;
using System;

namespace DuelEditor
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MachinaBootstrap.Run(new GameSpecification("Duel-editor", args, new GameSettings(new Point(1600, 900))), new DuelEditorCartridge(new Point(1600, 900)), ".");
        }
    }
}
