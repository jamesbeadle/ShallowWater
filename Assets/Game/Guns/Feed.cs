namespace ShallowWater.Game.Guns
{
    public abstract class Feed
    {
        protected Feed(Cartridges cartridges)
        {
            Cartridges = cartridges;
        }

        public Cartridges Cartridges { get; }
        public abstract bool IsLoaded { get; }
        public abstract string Words { get; }

        public abstract void Work(double seconds);

        public abstract void Fire();
    }
}
