namespace ShallowWater.Game.Houses
{
    public readonly struct Neighbours
    {
        public Neighbours(bool isOnTheLeft, bool isOnTheRight)
        {
            IsOnTheLeft = isOnTheLeft;
            IsOnTheRight = isOnTheRight;
        }

        public bool IsOnTheLeft { get; }
        public bool IsOnTheRight { get; }
        public bool IsDetached => !IsOnTheLeft && !IsOnTheRight;
    }
}
