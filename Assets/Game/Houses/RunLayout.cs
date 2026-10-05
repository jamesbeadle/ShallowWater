using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Houses
{
    public sealed class RunLayout
    {
        public RunLayout(HouseStyle style, double alongMetres, IReadOnlyList<double> frontages, bool isJoinedToThePrevious)
        {
            Style = style;
            AlongMetres = alongMetres;
            Frontages = frontages;
            IsJoinedToThePrevious = isJoinedToThePrevious;
        }

        public HouseStyle Style { get; }
        public double AlongMetres { get; }
        public IReadOnlyList<double> Frontages { get; }
        public bool IsJoinedToThePrevious { get; }
        public int HouseCount => Frontages.Count;
        public bool StandsAlone => Style.StandsAlone;
        public double LengthMetres => Frontages.Sum();
        public double EndMetres => AlongMetres + LengthMetres;
    }
}
