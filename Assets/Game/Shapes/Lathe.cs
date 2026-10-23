using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Shapes
{
    public static class Lathe
    {
        public static Shape Turned(WorldPoint foot, IReadOnlyList<ProfilePoint> profile, int sides)
        {
            return Sculpt.Rings(foot, Heights(profile), Turn.Whole(sides), (ring, around) => profile[ring].RadiusMetres);
        }

        public static Shape TurnedThrough(WorldPoint foot, IReadOnlyList<ProfilePoint> profile, int sides, double fromRadians, double toRadians)
        {
            return Sculpt.Rings(foot, Heights(profile), Turn.Through(fromRadians, toRadians, sides), (ring, around) => profile[ring].RadiusMetres);
        }

        private static IReadOnlyList<double> Heights(IReadOnlyList<ProfilePoint> profile)
        {
            return profile.Select(point => point.HeightMetres).ToList();
        }
    }
}
