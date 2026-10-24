using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Guns
{
    public abstract class Rifle
    {
        private readonly Feed feed;

        protected Rifle(Feed feed)
        {
            this.feed = feed;
        }

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Provenance { get; }
        public abstract double RangeMetres { get; }
        public abstract double BalanceFromTheButtMetres { get; }
        public abstract Offset EyeFromTheButt { get; }
        public abstract RifleSight Sight { get; }
        public bool HasATelescope => Sight.IsATelescope;
        public Cartridges Cartridges => feed.Cartridges;
        public bool IsLoaded => feed.IsLoaded;
        public string Readiness => feed.Words;
        public bool IsShouldered { get; private set; }

        public abstract SurfaceShapes Shapes();

        public void Shoulder(bool isShouldered)
        {
            IsShouldered = isShouldered;
        }

        public void Work(double seconds)
        {
            feed.Work(seconds);
        }

        public void Fire()
        {
            if (!IsLoaded) return;
            feed.Fire();
        }
    }
}
