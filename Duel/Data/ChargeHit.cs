using Microsoft.Xna.Framework;

namespace Duel.Data
{
    public readonly struct ChargeHit
    {
        public ChargeHit(Point standingPosition, Direction direction)
        {
            Direction = direction;
            StandingPosition = standingPosition;
            RammedPosition = standingPosition + direction.ToPoint();
        }

        public Direction Direction { get; }
        public Point StandingPosition { get; }
        public Point RammedPosition { get; }
    }
}
