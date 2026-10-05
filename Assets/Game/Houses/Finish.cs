using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public readonly struct Finish
    {
        public Finish(double eavesMetres, Surface walls, Surface roof, bool isHipped)
        {
            EavesMetres = eavesMetres;
            Walls = walls;
            Roof = roof;
            IsHipped = isHipped;
        }

        public double EavesMetres { get; }
        public Surface Walls { get; }
        public Surface Roof { get; }
        public bool IsHipped { get; }
    }
}
