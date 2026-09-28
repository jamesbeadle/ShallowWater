using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Cottages
{
    public sealed class RoofSlope
    {
        private const double RidgeLine = 0;
        private const double AtTheEaves = 0;
        private const double RightHandSide = 1;
        private const double Level = 1;

        private readonly Cottage cottage;
        private readonly double side;
        private readonly double ridgeMetres;

        public RoofSlope(Cottage cottage, double side, double ridgeMetres)
        {
            this.cottage = cottage;
            this.side = side;
            this.ridgeMetres = ridgeMetres;
        }

        private double ReachMetres => cottage.HalfLengthMetres + CottageForm.GableOverhangMetres;
        private double OutwardMetres => cottage.HalfWidthMetres + CottageForm.EavesOverhangMetres;
        private double EavesMetres => ridgeMetres - OutwardMetres * CottageForm.RoofRisePerMetre;
        private double LengthMetres => OutwardMetres * Math.Sqrt(Level + CottageForm.RoofRisePerMetre * CottageForm.RoofRisePerMetre);
        private bool IsOnTheRight => side == RightHandSide;

        public Shape FacingUp()
        {
            return Wound(IsOnTheRight);
        }

        public Shape FacingDown()
        {
            return Wound(!IsOnTheRight);
        }

        private Shape Wound(bool isForwards)
        {
            var slope = new Shape();
            var eavesLine = side * OutwardMetres;
            var backEaves = Corner(slope, -ReachMetres, eavesLine, EavesMetres, AtTheEaves);
            var frontEaves = Corner(slope, ReachMetres, eavesLine, EavesMetres, AtTheEaves);
            var backRidge = Corner(slope, -ReachMetres, RidgeLine, ridgeMetres, LengthMetres);
            var frontRidge = Corner(slope, ReachMetres, RidgeLine, ridgeMetres, LengthMetres);
            if (isForwards)
            {
                slope.AddQuad(backEaves, frontEaves, backRidge, frontRidge);
                return slope;
            }
            slope.AddQuad(frontEaves, backEaves, frontRidge, backRidge);
            return slope;
        }

        private int Corner(Shape slope, double along, double across, double heightMetres, double upTheSlope)
        {
            var ground = cottage.At(along, across);
            return slope.Add(new WorldPoint(ground.East, heightMetres, ground.North), new SurfacePlace(along, upTheSlope));
        }
    }
}
