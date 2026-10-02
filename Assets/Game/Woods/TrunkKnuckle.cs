using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TrunkKnuckle
    {
        private static readonly (double Reach, double Radius)[] Dome = { (0.5, 0.87), (0.85, 0.53), (1.0, 0.08) };

        public static Limb Knuckled(Limb trunk)
        {
            var tip = trunk.Tip;
            var path = trunk.Path.ToList();
            var radii = trunk.RadiiMetres.ToList();
            foreach (var ring in Dome)
            {
                path.Add((tip.Heading * (tip.RadiusMetres * ring.Reach)).From(tip.Place));
                radii.Add(tip.RadiusMetres * ring.Radius);
            }
            return new Limb(path, radii, trunk.Order);
        }
    }
}
