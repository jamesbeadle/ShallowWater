using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.People
{
    public static class Profiles
    {
        public static double RadiusAt(IReadOnlyList<ProfilePoint> profile, double heightMetres)
        {
            var first = profile[0];
            var isBelowTheProfile = heightMetres <= first.HeightMetres;
            if (isBelowTheProfile) return first.RadiusMetres;
            for (var point = 1; point < profile.Count; point++)
            {
                var isBelow = heightMetres <= profile[point].HeightMetres;
                if (isBelow) return Between(profile[point - 1], profile[point], heightMetres);
            }
            var last = profile[profile.Count - 1];
            return last.RadiusMetres;
        }

        private static double Between(ProfilePoint lower, ProfilePoint upper, double heightMetres)
        {
            var share = (heightMetres - lower.HeightMetres) / (upper.HeightMetres - lower.HeightMetres);
            return lower.RadiusMetres + (upper.RadiusMetres - lower.RadiusMetres) * share;
        }
    }
}
