using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class RailwayLook
    {
        private static readonly int Ballast = Shader.PropertyToID("_Ballast");
        private static readonly int BallastGrain = Shader.PropertyToID("_BallastGrain");
        private static readonly int Sleeper = Shader.PropertyToID("_Sleeper");
        private static readonly int Rail = Shader.PropertyToID("_Rail");

        public static Material Track()
        {
            var material = LookShaders.Made(LookShaders.Railway);
            var ballast = RoadPalette.Ballast;
            material.SetColor(Ballast, ballast.Main);
            material.SetColor(BallastGrain, ballast.Worn);
            material.SetColor(Sleeper, ballast.Accent);
            material.SetColor(Rail, RoadPalette.Rail);
            return material;
        }
    }
}
