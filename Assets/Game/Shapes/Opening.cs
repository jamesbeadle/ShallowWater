namespace ShallowWater.Game.Shapes
{
    public readonly struct Opening
    {
        private const double Half = 0.5;

        public Opening(double centreAlong, double widthMetres, double bottomMetres, double topMetres)
        {
            Left = centreAlong - widthMetres * Half;
            Right = centreAlong + widthMetres * Half;
            Bottom = bottomMetres;
            Top = topMetres;
        }

        public double Left { get; }
        public double Right { get; }
        public double Bottom { get; }
        public double Top { get; }
        public double CentreAlong => (Left + Right) * Half;
        public double MiddleHeight => (Bottom + Top) * Half;
        public double WidthMetres => Right - Left;
        public double HeightMetres => Top - Bottom;

        public bool IsOver(double along)
        {
            return along > Left && along < Right;
        }
    }
}
