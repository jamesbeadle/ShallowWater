using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class LeanToRoof
    {
        private const double FreeVergeMetres = 0.1;
        private const double NoVerge = 0;
        private const double DoubledForTheMirroredSlope = 2;
        private const double FromTheLeft = -1;
        private const double FromTheRight = 1;

        public static void Lay(SurfaceShapes surfaces, Run run, Plot outshut, Rise eaves, Neighbours partyWalls)
        {
            var depth = outshut.DepthMetres;
            var mirrored = outshut.Part(0, -depth, outshut.LengthMetres, depth * DoubledForTheMirroredSlope);
            var pitch = new Pitch(depth, eaves.TopMetres, eaves.HeightMetres / depth);
            var from = partyWalls.IsOnTheLeft ? NoVerge : -FreeVergeMetres;
            var to = outshut.LengthMetres + (partyWalls.IsOnTheRight ? NoVerge : FreeVergeMetres);
            var soffit = pitch.Lowered(RoofForm.SoffitBelowTheSlatesMetres);
            surfaces.Add(run.Roof, GabledRoof.Slope(mirrored, pitch, RoofForm.Back, from, to, true));
            surfaces.Add(Surface.Timber, GabledRoof.Slope(mirrored, soffit, RoofForm.Back, from, to, false));
            RoofEdges.AddFascia(surfaces, mirrored, pitch, RoofForm.Back, from, to);
            Gutters.Hang(surfaces, mirrored, pitch, RoofForm.Back, from, to);
            if (!partyWalls.IsOnTheLeft) RoofEdges.AddVergeBoard(surfaces, mirrored, pitch, RoofForm.Back, from, RoofEdges.PlainVergeMetres, FromTheLeft);
            if (!partyWalls.IsOnTheRight) RoofEdges.AddVergeBoard(surfaces, mirrored, pitch, RoofForm.Back, to, RoofEdges.PlainVergeMetres, FromTheRight);
        }
    }
}
