using Duel.Components;
using Machina.Engine;

namespace Duel.Data
{
    public abstract class Tag
    {
        public override string ToString()
        {
            return GetType().Name;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().Name.GetHashCode();
        }
    }

    public class BlockProjectileTag : Tag
    {
    }

    public class WaterFiller : Tag
    {
        public enum Type
        {
            Floats,
            Sinks
        }

        public Type FillerType { get; }
        public WaterFiller(Type fillerType)
        {
            FillerType = fillerType;
        }
    }

    public class Ravine : Tag
    {

    }

    public class UnfilledWater : Tag
    {
    }

    public class FilledWater : Tag
    {
        public Entity FillingEntity { get; private set; }

        public FilledWater(Entity entity)
        {
            FillingEntity = entity;
        }

    }

    public class ToggleSignal : Tag
    {
        public SignalColor Color { get; }

        private bool isOn;

        public bool IsOnBump { get; private set; }
        public bool IsOnHit { get; private set; }
        public bool IsOnGrapple { get; private set; }

        public ToggleSignal(SignalColor color)
        {
            Color = color;
            isOn = false;
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

        public void Toggle()
        {
            isOn = !isOn;
        }

        public bool IsOn()
        {
            return isOn;
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

    public class PressurePlateImageTag : Tag
    {
        public SignalColor Color { get; }

        public PressurePlateImageTag(SignalColor color)
        {
            Color = color;
        }
    }


    public class Key : Tag
    {
        public SignalColor Color { get; }

        public Key(SignalColor color)
        {
            Color = color;
        }
    }

    public class KeyDoor : Tag
    {
        public SignalColor Color { get; }

        public KeyDoor(SignalColor color)
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

    public class EditorImage : Tag
    {
        public EditorImage(EntityFrame frame)
        {
            EntityFrame = frame;
        }

        public EntityFrame EntityFrame { get; }
    }

    public class MiasmaImageTag : Tag
    {
    }

    public class SignalDoor : Tag
    {
        public SignalColor Color { get; }
        public bool DefaultOpened { get; }

        public SignalDoor(SignalColor signalColor, bool defaultOpened)
        {
            Color = signalColor;
            DefaultOpened = defaultOpened;
        }
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

        public SimpleEntityImage(EntityFrameSet entityFrameSet)
        {
            EntityFrameSet = entityFrameSet;
        }

        public EntityFrameSet EntityFrameSet { get; }
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