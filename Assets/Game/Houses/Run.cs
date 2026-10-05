using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class Run
    {
        private const double Half = 0.5;
        private const double NoVerge = 0;

        public Run(Plot plot, HouseStyle style, IReadOnlyList<House> houses, Finish finish, Neighbours neighbours)
        {
            Plot = plot;
            Style = style;
            Houses = houses;
            Finish = finish;
            Neighbours = neighbours;
        }

        public Plot Plot { get; }
        public HouseStyle Style { get; }
        public IReadOnlyList<House> Houses { get; }
        public Finish Finish { get; }
        public Neighbours Neighbours { get; }
        public double FootMetres => Heights.GroundMetres;
        public double EavesAboveTheGroundMetres => Finish.EavesMetres;
        public Surface Walls => Finish.Walls;
        public Surface Roof => Finish.Roof;
        public Lintel Lintel => Style.Lintel;
        public double RoofRisePerMetre => Style.RoofRisePerMetre;
        public double LengthMetres => Plot.LengthMetres;
        public double EavesMetres => FootMetres + Finish.EavesMetres;
        public double HalfDepthMetres => Plot.DepthMetres * Half;
        public double RidgeMetres => EavesMetres + HalfDepthMetres * RoofRisePerMetre;
        public Rise Storeys => new Rise(FootMetres, EavesMetres);
        public bool IsHipped => Finish.IsHipped;

        public IEnumerable<double> PartyWalls()
        {
            return Houses.Skip(1).Select(house => house.AlongMetres);
        }

        public double LeftVergeMetres => Neighbours.IsOnTheLeft ? NoVerge : Style.VergeMetres;
        public double RightVergeMetres => Neighbours.IsOnTheRight ? NoVerge : Style.VergeMetres;
    }
}
