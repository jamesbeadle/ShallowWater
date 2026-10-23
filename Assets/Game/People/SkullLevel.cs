using System;

namespace ShallowWater.Game.People
{
    public readonly struct SkullLevel
    {
        private const double Level = 0;

        public SkullLevel(double heightMetres, double frontMetres, double sideMetres, double backMetres)
        {
            HeightMetres = heightMetres;
            FrontMetres = frontMetres;
            SideMetres = sideMetres;
            BackMetres = backMetres;
        }

        public double HeightMetres { get; }
        public double FrontMetres { get; }
        public double SideMetres { get; }
        public double BackMetres { get; }

        public double RadiusTowards(double around)
        {
            var front = Math.Max(Level, Math.Sin(around));
            var back = Math.Max(Level, -Math.Sin(around));
            var side = Math.Cos(around);
            return FrontMetres * front * front + SideMetres * side * side + BackMetres * back * back;
        }
    }
}
