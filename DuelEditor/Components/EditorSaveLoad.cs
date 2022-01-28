using Duel.Data;
using DuelEditor.Components;
using DuelEditor.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuelEditor.Components
{
    public class EditorSaveLoad : BaseComponent
    {
        private Sokoban game;
        private string currentLevelName = null;

        public EditorSaveLoad(Actor actor, Sokoban game, EditorCore editor) : base(actor)
        {
            this.game = game;

            editor.LevelLoaded += SaveLevelName;
        }

        private void SaveLevelName(string levelName)
        {
            MachinaClient.Print("level loaded", levelName);
            this.currentLevelName = levelName;
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            if (key == Keys.S && state == ButtonState.Pressed && modifiers.Control)
            {
                Save();
            }
        }

        public void Save()
        {
            var fileTime = DateTime.Now.ToFileTimeUtc();
            var levelName = $"TEMPLEVEL{fileTime}";

            if (this.currentLevelName != null)
            {
                levelName = this.currentLevelName;
            }

            var fileName = $"levels/{levelName}.bunk";

            var builder = new StringBuilder();

            foreach (var instance in this.game.ActorRoot.GetAllInstances())
            {
                if (instance.TemplateName == "")
                {
                    MachinaClient.Print("warning: Entity does not have a templateName");
                }

                builder.Append($"{instance.TemplateClassName} {instance.TemplateName} {instance.CoordinateString}");
                builder.Append('\n');
            }

            MachinaClient.FileSystem.WriteStringToAppData(builder.ToString(), fileName, false);
        }

        public LevelData GetCurrentLevel()
        {
            return this.game.BuildDataFromCurrentLevel(this.currentLevelName ?? "nameless");
        }
    }
}
