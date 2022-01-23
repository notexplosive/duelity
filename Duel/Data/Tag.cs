using Machina.Engine;

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

    public class Solid : Tag
    {
        public bool IsPushOnBump { get; private set; }
        public bool IsPushOnHit { get; private set; }

        public Solid PushOnBump()
        {
            IsPushOnBump = true;
            return this;
        }

        public Solid PushOnHit()
        {
            IsPushOnHit = true;
            return this;
        }

        public Solid()
        {
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

    public enum TileFrame
    {
        Floor0,
        Floor1,
        Floor2,
        Floor3,
        Floor4,
        Wall,
        WaterLeftNub,
        WaterHorizontal,
        WaterRightNub,
        Hook,
        HookLassod,
        Bramble0,
        WaterTopNub,
        WaterLeftEdge,
        WaterTopEdge,
        WaterCenterNub,
        Bridge,
        Bramble1,
        WaterVerticalNub,
        WaterRightEdge,
        WaterBottomEdge,
        BBB,
        CCC,
        DDD,
        WaterBottomNub,
        WaterAloneNub,
        WaterCenter,
        EEE,
        FFF,
        GGG,
        WaterTopRight,
        WaterTopLeft,
        WaterBottomRight,
        WaterBottomLeft,
        HHH,
        III,
        RavineLeftNub,
        RavineHorizontal,
        RavineRightNub,
        JJJ,
        KKK,
        LLL,
        RavineTopNub,
        RavineLeftEdge,
        RavineTopEdge,
        RavineCenterNub,
        MMM,
        NNN,
        RavineVerticalNub,
        RavineRightEdge,
        RavineBottomEdge,
        OOO,
        PPP,
        QQQ,
        RavineBottomNub,
        RavineAloneNub,
        RavineCenter,
        RRR,
        SSS,
        TTT,
        RavineTopRight,
        RavineTopLeft,
        RavineBottomRight,
        RavineBottomLeft,
    }

    public class TileImageTag : Tag
    {
        public TileImage Image { get; }

        public enum TileImage
        {
            Floor,
            Wall,
            Water,
            Hook,
            Ravine,
            Bramble,
            Bridge
        }

        public TileImageTag(TileImage image)
        {
            Image = image;
        }
    }

    public class Grapplable : Tag
    {
        public Type HookType { get; }

        public enum Type
        {
            Static, // Moves player to there
            PulledByLasso, // Lasso yanks it to the player
        }

        public Grapplable(Type type)
        {
            HookType = type;
        }
    }

    /// <summary>
    /// Thing responds to being shot, does not block the projectile
    /// </summary>
    public class DestroyOnHit : Tag
    {
        public DestroyOnHit()
        {
        }
    }
}