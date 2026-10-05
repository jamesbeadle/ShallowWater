using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class HippedRoof
    {
        private const double Half = 0.5;

        public static void Lay(SurfaceShapes surfaces, Run run)
        {
            var plot = run.Plot;
            var pitch = Pitch.Of(run);
            var soffit = pitch.Lowered(RoofForm.SoffitBelowTheSlatesMetres);
            foreach (var face in HipFaces.Of(plot, pitch))
            {
                surfaces.Add(run.Roof, face.Covering(plot, pitch, true));
                surfaces.Add(Surface.Timber, face.Covering(plot, soffit, false));
            }
            foreach (var (side, sidePlot, sidePitch) in Sides(plot, pitch))
            {
                var reach = sidePitch.OutToTheEavesMetres - sidePitch.HalfSpanMetres;
                RoofEdges.AddFascia(surfaces, sidePlot, sidePitch, side, -reach, sidePlot.LengthMetres + reach);
                Gutters.Hang(surfaces, sidePlot, sidePitch, side, -reach, sidePlot.LengthMetres + reach);
            }
            LayHipTiles(surfaces, plot, pitch);
        }

        private static IEnumerable<(double, Plot, Pitch)> Sides(Plot plot, Pitch pitch)
        {
            foreach (var side in RoofForm.Sides) yield return (side, plot, pitch);
            var halfLength = plot.LengthMetres * Half;
            var endwise = new Plot(plot.At(0, plot.DepthMetres), plot.Backwards * -1, plot.DepthMetres, plot.LengthMetres);
            var endPitch = new Pitch(halfLength, pitch.EavesMetres + (halfLength + RoofForm.EavesOverhangMetres) * pitch.RisePerMetre, pitch.RisePerMetre);
            foreach (var side in RoofForm.Sides) yield return (side, endwise, endPitch);
        }

        private static void LayHipTiles(SurfaceShapes surfaces, Plot plot, Pitch pitch)
        {
            var middle = pitch.HalfSpanMetres;
            var ridgeStart = Math.Min(middle, plot.LengthMetres * Half);
            var ridgeEnd = plot.LengthMetres - ridgeStart;
            var left = plot.Point(ridgeStart, middle, pitch.RidgeMetres);
            var right = plot.Point(ridgeEnd, middle, pitch.RidgeMetres);
            RidgeTiles.Lay(surfaces, left, right, plot.Backwards);
            var reach = RoofForm.EavesOverhangMetres;
            var eaves = pitch.EavesMetres;
            foreach (var side in RoofForm.Sides)
            {
                var line = pitch.EavesLine(side);
                LayHip(surfaces, plot.Point(-reach, line, eaves), left);
                LayHip(surfaces, plot.Point(plot.LengthMetres + reach, line, eaves), right);
            }
        }

        private static void LayHip(SurfaceShapes surfaces, WorldPoint foot, WorldPoint top)
        {
            var run = new GroundPoint(top.East - foot.East, top.North - foot.North);
            var across = run.RightAngleClockwise;
            RidgeTiles.Lay(surfaces, foot, top, across.Normalised);
        }
    }
}
