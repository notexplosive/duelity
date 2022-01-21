using Machina.Data;
using Machina.Engine;
using MachinaDesktop;
using Microsoft.Xna.Framework;
using System;

namespace Duel
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MachinaBootstrap.Run(new GameSpecification("Duel-ity", args, new GameSettings(new Point(1600, 900))), new DuelGameCartridge(), ".");
        }
    }
}
