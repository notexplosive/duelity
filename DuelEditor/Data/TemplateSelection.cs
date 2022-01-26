using Duel.Data;
using System;

namespace DuelEditor.Data
{
    public class TemplateSelection
    {
        public IEntityOrTileTemplate Primary { get; set; }

        public bool IsInEntityMode => Primary is EntityTemplate;
        public bool IsInTileMode => Primary is TileTemplate;
    }
}
