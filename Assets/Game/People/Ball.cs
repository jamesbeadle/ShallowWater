using System;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Ball
    {
        private const int Rings = 6;

        public static Shape Of(WorldPoint centre, double radiusMetres, int sides)
        {
            var profile = Enumerable.Range(0, Rings + 1).Select(ring =>
            {
                var fromTheBottom = Math.PI * ring / Rings;
                return new ProfilePoint(radiusMetres * Math.Sin(fromTheBottom), -radiusMetres * Math.Cos(fromTheBottom));
            }).ToArray();
            return Lathe.Turned(centre, profile, sides);
        }
    }
}
