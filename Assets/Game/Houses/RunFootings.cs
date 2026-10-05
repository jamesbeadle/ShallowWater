using System.Collections.Generic;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Houses
{
    public static class RunFootings
    {
        private const double Half = 0.5;

        public static IEnumerable<GroundRing> Of(Run run)
        {
            var plot = run.Plot;
            var style = run.Style;
            yield return Covering(plot, 0, 0, plot.LengthMetres, plot.DepthMetres);
            foreach (var house in run.Houses)
            {
                var extras = house.Extras;
                var outshut = style.OutshutOf(house);
                var isBuilt = extras.HasOutshut && outshut.IsBuilt;
                if (isBuilt) yield return Covering(plot, outshut.AlongMetres, plot.DepthMetres, outshut.WidthMetres, style.OutshutDepthMetres);
            }
        }

        private static GroundRing Covering(Plot plot, double along, double back, double lengthMetres, double depthMetres)
        {
            return plot.Oblong(along + lengthMetres * Half, back + depthMetres * Half, lengthMetres * Half, depthMetres * Half);
        }
    }
}
