using System;

namespace ShallowWater.Game.Houses
{
    public readonly struct Pitch
    {
        public Pitch(double halfSpanMetres, double ridgeMetres, double risePerMetre)
        {
            HalfSpanMetres = halfSpanMetres;
            RidgeMetres = ridgeMetres;
            RisePerMetre = risePerMetre;
        }

        public double HalfSpanMetres { get; }
        public double RidgeMetres { get; }
        public double RisePerMetre { get; }
        public double OutToTheEavesMetres => HalfSpanMetres + RoofForm.EavesOverhangMetres;
        public double EavesMetres => RidgeMetres - OutToTheEavesMetres * RisePerMetre;
        public double UpTheSlopeMetres => OutToTheEavesMetres * Math.Sqrt(1 + RisePerMetre * RisePerMetre);

        public double EavesLine(double side)
        {
            return HalfSpanMetres + side * OutToTheEavesMetres;
        }

        public Pitch Lowered(double metres)
        {
            return new Pitch(HalfSpanMetres, RidgeMetres - metres, RisePerMetre);
        }

        public static Pitch Of(Run run)
        {
            return new Pitch(run.HalfDepthMetres, run.RidgeMetres, run.RoofRisePerMetre);
        }
    }
}
