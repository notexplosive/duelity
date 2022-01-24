using Duel.Data;
using Machina.Components;
using Machina.Data;
using Machina.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Duel.Components
{
    public enum EntityFrame
    {
        GlassHooch,
        Crate,
        Barrel,
        Anvil,
        Miasma,
        RedDoor,
        BlueDoor,
        YellowDoor,
        RedLockedDoor,
        BlueLockedDoor,
        YellowLockedDoor,
        RedLeverRight,
        BleLeverRight,
        YellowLeverRight,
        RedKey,
        BlueKey,
        YellowKey,
        RedPlateUp,
        BluePlateUp,
        YellowPlateUp,
        // next row
        GlassHoochLassod,
        CrateLassod,
        BarrelLassod,
        AAA,
        BBB,
        RedDoorOpen,
        BlueDoorOpen,
        YellowDoorOpen,
        CCC,
        DDD,
        EEE,
        RedLeverLeft,
        BlueLeverLeft,
        YellowLeverLeft,
        RedKeyLassod,
        BlueKeyLassod,
        YellowKeyLassod,
        RedPlateDown,
        BluePlateDown,
        YellowPlateDown,
        // next row
        GlassHoochBreak,
        CrateBreak
    }

    public class SimpleEntityRenderer : BaseComponent
    {
        private readonly SimpleEntityImage.EntityFrameSet frameSet;
        private readonly Entity entity;
        private readonly MovementRenderer movement;
        private readonly EntityRenderInfo renderInfo;
        private readonly Actor spriteActor;
        private readonly SpriteRenderer spriteRenderer;

        public SimpleEntityRenderer(Actor actor, SimpleEntityImage.EntityFrameSet frameSet, Entity entity) :
            base(actor)
        {
            this.frameSet = frameSet;
            this.entity = entity;

            this.entity.GrabbedByLasso += BecomeLassodSprite;
            this.entity.ReleasedFromLasso += RestoreNormalSprite;

            this.movement = RequireComponent<MovementRenderer>();
            this.renderInfo = RequireComponent<EntityRenderInfo>();

            this.spriteActor = transform.AddActorAsChild("sprite");
            this.spriteRenderer = new SpriteRenderer(this.spriteActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet"));
            this.spriteRenderer.FramesPerSecond = 0;
            this.spriteRenderer.SetFrame(this.frameSet.Normal);
            this.renderInfo.DisableDebugGraphic();
        }

        private void RestoreNormalSprite()
        {
            this.spriteRenderer.SetFrame(this.frameSet.Normal);
        }

        private void BecomeLassodSprite()
        {
            this.spriteRenderer.SetFrame(this.frameSet.Lassod);
        }

        public override void OnActorDestroy()
        {
            var debrisActor = this.actor.transform.Parent.AddActorAsChild("DeadSprite");
            var debrisSpriteRenderer = new SpriteRenderer(debrisActor, MachinaClient.Assets.GetMachinaAsset<SpriteSheet>("entities-sheet"));
            debrisSpriteRenderer.FramesPerSecond = 0;
            debrisSpriteRenderer.SetFrame(this.frameSet.Broken);
            debrisActor.transform.Position = transform.Position + RenderOffset();
            new DebrisDestroy(debrisActor);
        }

        public override void OnKey(Keys key, ButtonState state, ModifierKeys modifiers)
        {
            // technically we don't need to do this, but if we're in framestep it looks weird if we don't
            BindSpritePosToRenderOffset();
        }

        public override void Update(float dt)
        {
            BindSpritePosToRenderOffset();
        }

        private void BindSpritePosToRenderOffset()
        {
            this.spriteRenderer.SetOffset(RenderOffset());
        }

        private Vector2 RenderOffset()
        {
            return -this.renderInfo.renderOffsetTweenable.getter();
        }
    }
}