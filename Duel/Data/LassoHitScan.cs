using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public class LassoHitScan
    {
        public Point LassoLandingPosition { get; }
        public bool FoundHook { get; }
        public bool Invalid { get; }
        public Entity EntityToPull { get; } = null;

        public LassoHitScan(Point startingPosition, Direction throwDirection, LevelSolidProvider solidProvider)
        {
            LassoLandingPosition = startingPosition;
            FoundHook = false;

            if (solidProvider.HasTagAt<BlockProjectileTag>(startingPosition + throwDirection.ToPoint()))
            {
                Invalid = true;
            }

            bool wasBlocked = false;

            for (int i = 0; i < 3; i++)
            {
                LassoLandingPosition += throwDirection.ToPoint();
                var nextPos = LassoLandingPosition + throwDirection.ToPoint();

                if (solidProvider.HasTagAt<Grapplable>(LassoLandingPosition))
                {
                    FoundHook = true;
                }

                if (solidProvider.HasTagAt<BlockProjectileTag>(nextPos))
                {
                    wasBlocked = true;
                }

                if (FoundHook || wasBlocked)
                {
                    break;
                }
            }

            if (solidProvider.TryGetFirstEntityWithTagAt<Grapplable>(LassoLandingPosition, out Entity grapplableEntity))
            {
                if (grapplableEntity.Tags.GetTag<Grapplable>().HookType == Grapplable.Type.PulledByLasso)
                {
                    EntityToPull = grapplableEntity;
                }
            }
        }
    }
}