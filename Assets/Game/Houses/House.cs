namespace ShallowWater.Game.Houses
{
    public sealed class House
    {
        private const double Half = 0.5;

        public House(double alongMetres, double widthMetres, bool isDoorOnTheRight, Paintwork paintwork, Extras extras)
        {
            AlongMetres = alongMetres;
            WidthMetres = widthMetres;
            IsDoorOnTheRight = isDoorOnTheRight;
            Paintwork = paintwork;
            Extras = extras;
        }

        public double AlongMetres { get; }
        public double WidthMetres { get; }
        public bool IsDoorOnTheRight { get; }
        public Paintwork Paintwork { get; }
        public Extras Extras { get; }
        public double MiddleMetres => AlongMetres + WidthMetres * Half;
        public double EndMetres => AlongMetres + WidthMetres;

        public double FromTheDoorSide(double metres)
        {
            return IsDoorOnTheRight ? EndMetres - metres : AlongMetres + metres;
        }

        public double FromTheFarSide(double metres)
        {
            return IsDoorOnTheRight ? AlongMetres + metres : EndMetres - metres;
        }
    }
}
