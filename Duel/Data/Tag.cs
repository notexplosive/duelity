using Duel.Components;
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

    public class ToggleSignal : Tag
    {
        public SignalColor Color { get; }
        public bool IsOnBump { get; private set; }
        public bool IsOnHit { get; private set; }
        public bool IsOnGrapple { get; private set; }

        public ToggleSignal(SignalColor color)
        {
            Color = color;
        }

        public ToggleSignal OnBump()
        {
            IsOnBump = true;
            return this;
        }

        public ToggleSignal OnHit()
        {
            IsOnHit = true;
            return this;
        }

        public ToggleSignal OnGrapple()
        {
            IsOnGrapple = true;
            return this;
        }
    }

    public class EnableSignalWhenSteppedOn : Tag
    {
        public EnableSignalWhenSteppedOn(SignalColor color)
        {
            Color = color;
        }

        public SignalColor Color { get; }
    }

    public class LeverImageTag : Tag
    {
        public SignalColor Color { get; }

        public LeverImageTag(SignalColor color)
        {
            Color = color;
        }
    }


    public enum SignalColor
    {
        Red,
        Blue,
        Yellow
    }

    public class MiasmaImageTag : Tag
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
    }

    public class SimpleEntityImage : Tag
    {
        public class EntityFrameSet
        {
            public int Normal { get; }
            public int Lassod { get; }
            public int Broken { get; }

            public EntityFrameSet(EntityFrame normal, EntityFrame lassod, EntityFrame broken)
            {
                Normal = (int)normal;
                Lassod = (int)lassod;
                Broken = (int)broken;
            }

            public static readonly EntityFrameSet GlassBottle = new EntityFrameSet(EntityFrame.GlassHooch, EntityFrame.GlassHoochLassod, EntityFrame.GlassHoochBreak);
            public static readonly EntityFrameSet Crate = new EntityFrameSet(EntityFrame.Crate, EntityFrame.CrateLassod, EntityFrame.CrateBreak);
            public static readonly EntityFrameSet Anvil = new EntityFrameSet(EntityFrame.Anvil, EntityFrame.Anvil, EntityFrame.Anvil);
            public static readonly EntityFrameSet Barrel = new EntityFrameSet(EntityFrame.Barrel, EntityFrame.Barrel, EntityFrame.Barrel);
        }

        public SimpleEntityImage(EntityFrameSet entityFrameSet)
        {
            EntityClass = entityFrameSet;
        }

        public EntityFrameSet EntityClass { get; }
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

    public class Collapses : Tag
    {
        public TileTemplate TemplateAfterCollapse { get; }

        public Collapses(TileTemplate templateAfterCollapse)
        {
            this.TemplateAfterCollapse = templateAfterCollapse;
        }
    }
}