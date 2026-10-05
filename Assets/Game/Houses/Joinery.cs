using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public readonly struct Joinery
    {
        public Joinery(JoineryKind kind, Opening opening, int pattern, int paint)
        {
            Kind = kind;
            Opening = opening;
            Pattern = pattern;
            Paint = paint;
        }

        public JoineryKind Kind { get; }
        public Opening Opening { get; }
        public int Pattern { get; }
        public int Paint { get; }
        public bool IsDoor => Kind == JoineryKind.Door;
        public Fitting Fitting => new Fitting(Opening.WidthMetres, Opening.HeightMetres, Pattern, Paint);

        public static Joinery Window(Opening opening, int pattern, Paintwork paintwork)
        {
            return new Joinery(JoineryKind.Window, opening, pattern, paintwork.Frames);
        }

        public static Joinery Door(Opening opening, int pattern, Paintwork paintwork)
        {
            return new Joinery(JoineryKind.Door, opening, pattern, paintwork.Door);
        }
    }
}
