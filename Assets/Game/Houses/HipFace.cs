using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class HipFace
    {
        private readonly IReadOnlyList<(double along, double back)> corners;
        private readonly (double along, double back) eavesCorner;
        private readonly (double along, double back) eavesward;

        public HipFace(IReadOnlyList<(double along, double back)> corners, (double along, double back) eavesward)
        {
            this.corners = corners;
            this.eavesward = eavesward;
            eavesCorner = corners[0];
        }

        public Shape Covering(Plot plot, Pitch pitch, bool isFacingUp)
        {
            var slope = Math.Sqrt(1 + pitch.RisePerMetre * pitch.RisePerMetre);
            var points = corners.Select(corner => plot.Point(corner.along, corner.back, pitch.EavesMetres + Inward(corner) * pitch.RisePerMetre)).ToList();
            var places = corners.Select(corner => new SurfacePlace(Eavesward(corner), Inward(corner) * slope)).ToList();
            var centre = (along: corners.Average(corner => corner.along), back: corners.Average(corner => corner.back));
            var height = pitch.EavesMetres + Inward(centre) * pitch.RisePerMetre;
            var lookingFrom = isFacingUp ? RoofForm.ViewingHeightMetres : -RoofForm.ViewingHeightMetres;
            var covering = new Shape();
            Facet.Add(covering, points, places, plot.Point(centre.along, centre.back, height + lookingFrom));
            return covering;
        }

        private double Eavesward((double along, double back) corner)
        {
            return (corner.along - eavesCorner.along) * eavesward.along + (corner.back - eavesCorner.back) * eavesward.back;
        }

        private double Inward((double along, double back) corner)
        {
            return (corner.along - eavesCorner.along) * -eavesward.back + (corner.back - eavesCorner.back) * eavesward.along;
        }
    }
}
