using Duel.Components;
using System;

namespace Duel.Data
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

        public static EntityFrameSet Key(SignalColor color)
        {
            if (color == SignalColor.Red)
            {
                return new EntityFrameSet(EntityFrame.RedKey, EntityFrame.RedKeyLassod, EntityFrame.RedKey);
            }
            else if (color == SignalColor.Blue)
            {
                return new EntityFrameSet(EntityFrame.BlueKey, EntityFrame.BlueKeyLassod, EntityFrame.BlueKey);
            }
            else if (color == SignalColor.Yellow)
            {
                return new EntityFrameSet(EntityFrame.YellowKey, EntityFrame.YellowKeyLassod, EntityFrame.YellowKey);
            }

            throw new Exception($"Invalid signal color {color}");
        }

        public static EntityFrameSet KeyDoor(SignalColor color)
        {
            if (color == SignalColor.Red)
            {
                return new EntityFrameSet(EntityFrame.RedLockedDoor, EntityFrame.RedLockedDoor, EntityFrame.RedDoorOpen);
            }
            else if (color == SignalColor.Blue)
            {
                return new EntityFrameSet(EntityFrame.BlueLockedDoor, EntityFrame.BlueLockedDoor, EntityFrame.BlueDoorOpen);
            }
            else if (color == SignalColor.Yellow)
            {
                return new EntityFrameSet(EntityFrame.YellowLockedDoor, EntityFrame.YellowLockedDoor, EntityFrame.YellowDoorOpen);
            }

            throw new Exception($"Invalid signal color {color}");
        }
    }
}