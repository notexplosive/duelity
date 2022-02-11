﻿using Machina.Data.TextRendering;
using Microsoft.Xna.Framework;

namespace Duel.Data.Dialog
{
    public class Say : IDialogEvent
    {
        public Speaker Speaker { get; }
        public string RawText { get; }
        public Color Color { get; }

        public Say(Speaker speaker, string rawText, Color color)
        {
            Speaker = speaker;
            RawText = rawText;
            Color = color;
        }

        public FormattedText GetFormattedText(SpriteFontMetrics font)
        {
            return new FormattedText(new TextInputFragment(RawText, font, Color));
        }

        public FormattedText GetFormattedTextNoFont()
        {
            return new FormattedText(new TextInputFragment(RawText, new MonospacedFontMetrics(new Point(1)), Color));
        }
    }
}