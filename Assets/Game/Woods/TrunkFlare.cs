using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TrunkFlare
    {
        private const double FlareRingMetres = 0.45;
        private const double MostOfTheFirstSegment = 0.6;

        public static Limb Flared(Limb trunk, double flareShare)
        {
            var ringShare = Math.Min(FlareRingMetres / trunk.LengthMetres, MostOfTheFirstSegment / trunk.Segments);
            var ring = trunk.At(ringShare);
            var path = new List<WorldPoint> { trunk.Path[0], ring.Place };
            path.AddRange(trunk.Path.Skip(1));
            var radii = new List<double> { trunk.RadiiMetres[0] * (1 + flareShare), ring.RadiusMetres };
            radii.AddRange(trunk.RadiiMetres.Skip(1));
            return new Limb(path, radii, trunk.Order);
        }
    }
}
