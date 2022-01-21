﻿using Machina.Engine;

namespace Duel.Data
{
    public abstract class Tag
    {
        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode();
        }
    }

    public class SpriteTag : Tag
    {
    }

    public class BlockProjectileTag : Tag
    {
    }

    public class SolidTag : Tag
    {
        public Type SolidType { get; }

        public enum Type
        {
            Static,
            Pushable // only makes sense on entities... I think?
        }

        public SolidTag(Type type)
        {
            SolidType = type;
        }
    }

    public class PlayerTag : Tag
    {
        public Type MovementType { get; }

        public enum Type
        {
            Sheriff,
            Renegade,
            Cowboy,
            Knight
        }

        public PlayerTag(Type movementType)
        {
            MovementType = movementType;
        }
    }

    public class TileImageTag : Tag
    {
        public TileImage Image { get; }

        public enum TileImage
        {
            Floor,
            Wall
        }

        public TileImageTag(TileImage image)
        {
            Image = image;
        }
    }
}