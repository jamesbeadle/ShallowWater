using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Farmyard
{
    public static class Hayrick
    {
        private const int Sides = 16;
        private const double Radius = FarmyardPlan.RickRadiusMetres;
        private const double EavesOverhangMetres = 0.3;
        private const double ThatchShoulderShare = 0.62;
        private const double ApexRadiusMetres = 0.15;

        private static readonly ProfilePoint[] Stack =
        {
            new ProfilePoint(Radius, 0), new ProfilePoint(Radius + 0.1, 1.2), new ProfilePoint(Radius, 2.9)
        };

        private static readonly ProfilePoint[] Thatch =
        {
            new ProfilePoint(Radius + EavesOverhangMetres, 2.8), new ProfilePoint(Radius * ThatchShoulderShare, 3.9),
            new ProfilePoint(ApexRadiusMetres, FarmyardPlan.RickApexMetres)
        };

        public static void Stand(SurfaceShapes shapes, WorldPoint foot)
        {
            shapes.Add(Surface.Straw, Lathe.Turned(foot, Stack, Sides));
            shapes.Add(Surface.Thatch, Lathe.Turned(foot, Thatch, Sides));
        }
    }
}
