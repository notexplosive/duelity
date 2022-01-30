using Duel.Components;
using Machina.Engine;
using Microsoft.Xna.Framework.Audio;

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
        private readonly SoundEffectInstance onProjectileHitSound;

        public BlockProjectileTag(string hitSoundName = null)
        {
            if (hitSoundName != null)
            {
                this.onProjectileHitSound = MachinaClient.Assets.GetSoundEffectInstance(hitSoundName);
            }
        }

        public void PlayHitSound()
        {
            if (this.onProjectileHitSound != null)
            {
                this.onProjectileHitSound.Stop();
                this.onProjectileHitSound.Play();
            }
        }
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

    public class ZoneTransitionTrigger : Tag
    {
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
        private SoundEffectInstance pushSound;

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

        public Solid SetPushSound(string pushSoundName)
        {
            this.pushSound = MachinaClient.Assets.GetSoundEffectInstance(pushSoundName);
            return this;
        }

        public void PlayPushSound()
        {
            if (this.pushSound != null)
            {
                this.pushSound.Stop();
                this.pushSound.Play();
            }
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


    public class PlayerSpawn : Tag
    {
        public PlayerSpawn(PlayerTag.Type player)
        {
            Player = player;
        }

        public PlayerTag.Type Player { get; }
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
        BridgeOverRavine,
        DDD,
        WaterBottomNub,
        WaterAloneNub,
        WaterCenter,
        EEE,
        FFF,
        OasisWall,
        WaterTopRight,
        WaterTopLeft,
        WaterBottomRight,
        WaterBottomLeft,
        HHH,
        III,
        RavineLeftNub,
        RavineHorizontal,
        RavineRightNub,
        CrackedWall,
        OasisCrackedWall,
        MineCrackedWall,
        RavineTopNub,
        RavineLeftEdge,
        RavineTopEdge,
        RavineCenterNub,
        MMM,
        NNN,
        RavineVerticalNub,
        RavineRightEdge,
        RavineBottomEdge,
        Minefloor,
        PPP,
        QQQ,
        RavineBottomNub,
        RavineAloneNub,
        RavineCenter,
        RRR,
        SSS,
        Minewall,
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
            Bridge,
            BridgeOverRavine
        }

        public TileImageTag(TileImage image)
        {
            Image = image;
        }
    }

    // this keys into the npc spritesheet
    public enum NpcSprite
    {
        Sar,
        MirandaMom,
        Farmer,
        Cactus,
        Jackalope,
        EggMan,
        CoolHair,
        WhiteDressLady,
        Miner,
        Moleman,
        Mime,
        FourMan,
        King
    }

    public class NpcTag : Tag
    {
        private readonly string sheriffConvoKey;
        private readonly string renegadeConvoKey;
        private readonly string cowboyConvoKey;
        private readonly string knightConvoKey;

        public NpcTag(NpcSprite sprite, string sheriffConvoKey, string renegadeConvoKey, string cowboyConvoKey, string knightConvoKey)
        {
            Sprite = sprite;
            this.sheriffConvoKey = sheriffConvoKey;
            this.renegadeConvoKey = renegadeConvoKey;
            this.cowboyConvoKey = cowboyConvoKey;
            this.knightConvoKey = knightConvoKey;
        }

        public string GetConversationKey(PlayerTag.Type type)
        {
            switch (type)
            {
                case PlayerTag.Type.Sheriff:
                    return sheriffConvoKey;
                case PlayerTag.Type.Renegade:
                    return renegadeConvoKey;
                case PlayerTag.Type.Cowboy:
                    return cowboyConvoKey;
                case PlayerTag.Type.Knight:
                    return knightConvoKey;
            }

            throw new System.Exception("pththppppththphh");
        }

        public NpcSprite Sprite { get; }
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
        private readonly SoundEffectInstance destroySound;

        public DestroyOnHit(string destroySound = null)
        {
            if (destroySound != null)
            {
                this.destroySound = MachinaClient.Assets.GetSoundEffectInstance(destroySound);
            }
        }

        public void PlaySound()
        {
            if (this.destroySound != null)
            {
                this.destroySound.Volume = 0.5f;
                this.destroySound.Stop();
                this.destroySound.Play();
            }
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