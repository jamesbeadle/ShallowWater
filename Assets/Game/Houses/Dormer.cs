using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class Dormer
    {
        private const double ViewingDistanceMetres = 1;
        private const double Facade = 0;

        private readonly Run run;

        public Dormer(Run run, double centreMetres, double halfWidthMetres, double eavesMetres, double risePerMetre)
        {
            this.run = run;
            CentreMetres = centreMetres;
            HalfWidthMetres = halfWidthMetres;
            EavesMetres = eavesMetres;
            RisePerMetre = risePerMetre;
        }

        public double CentreMetres { get; }
        public double HalfWidthMetres { get; }
        public double EavesMetres { get; }
        public double RisePerMetre { get; }
        public double RidgeMetres => EavesMetres + HalfWidthMetres * RisePerMetre;
        public Plot Plot => run.Plot;

        public double BackWhereTheRoofMeets(double heightMetres)
        {
            return (heightMetres - run.EavesMetres) / run.RoofRisePerMetre;
        }

        public WallFace Front()
        {
            var plot = run.Plot;
            var rise = new Rise(run.EavesMetres, EavesMetres);
            return new WallFace(plot.At(CentreMetres - HalfWidthMetres, Facade), plot.At(CentreMetres + HalfWidthMetres, Facade), rise);
        }

        public Shape Cheek(double side)
        {
            var plot = run.Plot;
            var along = CentreMetres + side * HalfWidthMetres;
            var back = BackWhereTheRoofMeets(EavesMetres);
            var corners = new[] { plot.Point(along, Facade, run.EavesMetres), plot.Point(along, Facade, EavesMetres), plot.Point(along, back, EavesMetres) };
            var places = new[] { new SurfacePlace(Facade, run.EavesMetres), new SurfacePlace(Facade, EavesMetres), new SurfacePlace(back, EavesMetres) };
            var cheek = new Shape();
            Facet.Add(cheek, corners, places, plot.Point(along + side * ViewingDistanceMetres, back, EavesMetres));
            return cheek;
        }
    }
}
