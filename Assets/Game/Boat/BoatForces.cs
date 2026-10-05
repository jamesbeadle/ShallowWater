namespace ShallowWater.Game.Boat
{
    public readonly struct BoatForces
    {
        public static readonly BoatForces None = new BoatForces(0, 0, 0);

        public BoatForces(double ahead, double toStarboard, double swinging)
        {
            Ahead = ahead;
            ToStarboard = toStarboard;
            Swinging = swinging;
        }

        public double Ahead { get; }
        public double ToStarboard { get; }
        public double Swinging { get; }

        public static BoatForces Aside(double aheadMetres, double toStarboard)
        {
            return new BoatForces(0, toStarboard, aheadMetres * toStarboard);
        }

        public static BoatForces operator +(BoatForces first, BoatForces second)
        {
            var ahead = first.Ahead + second.Ahead;
            var toStarboard = first.ToStarboard + second.ToStarboard;
            return new BoatForces(ahead, toStarboard, first.Swinging + second.Swinging);
        }
    }
}
