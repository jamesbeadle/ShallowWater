using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class Lintels
    {
        private const double ArchOverhangMetres = 0.06;
        private const double ArchRingMetres = 0.23;
        private const double ArchRiseShareOfTheSpan = 0.08;
        private const double ArchProudMetres = 0.012;
        private const int ArchSteps = 10;
        private const double StoneOverhangMetres = 0.1;
        private const double StoneDepthMetres = 0.22;
        private const double StoneProudMetres = 0.03;
        private const double FlushWithTheWall = 0;
        private const double KeystoneHalfWidthMetres = 0.09;
        private const double KeystoneDropMetres = 0.03;
        private const double KeystoneRiseMetres = 0.3;
        private const double KeystoneProudMetres = 0.055;
        private const double Flat = 0;

        public static void Add(SurfaceShapes surfaces, WallFace face, Opening opening, Surface brick, Lintel lintel)
        {
            if (lintel == Lintel.StoneWithKeystone)
            {
                AddStone(surfaces, face, opening);
                return;
            }
            var rise = lintel == Lintel.CambredArch ? opening.WidthMetres * ArchRiseShareOfTheSpan : Flat;
            surfaces.Add(brick, Arch(face, opening, rise));
        }

        private static Shape Arch(WallFace face, Opening opening, double riseMetres)
        {
            var proud = face.Inset(-ArchProudMetres);
            var left = opening.Left - ArchOverhangMetres;
            var span = opening.WidthMetres + ArchOverhangMetres * 2;
            var springing = opening.Top;
            var corners = new List<WorldPoint> { proud.Point(left + span, springing), proud.Point(left, springing) };
            var places = new List<SurfacePlace> { new SurfacePlace(span, 0), new SurfacePlace(0, 0) };
            for (var step = 0; step <= ArchSteps; step++)
            {
                var share = (double)step / ArchSteps;
                var crown = springing + ArchRingMetres + riseMetres * Math.Sin(Math.PI * share);
                corners.Add(proud.Point(left + span * share, crown));
                places.Add(new SurfacePlace(span * share, crown - springing));
            }
            var arch = new Shape();
            Facet.Add(arch, corners, places, face.SeenFrom(opening.CentreAlong, springing));
            return arch;
        }

        private static void AddStone(SurfaceShapes surfaces, WallFace face, Opening opening)
        {
            var rise = face.Rise;
            var head = rise.FootMetres + opening.Top;
            var lintel = face.Slab(opening.Left - StoneOverhangMetres, opening.Right + StoneOverhangMetres, FlushWithTheWall, StoneProudMetres);
            surfaces.AddBlock(Surface.Dressing, lintel, head, head + StoneDepthMetres);
            var middle = opening.CentreAlong;
            var keystone = face.Slab(middle - KeystoneHalfWidthMetres, middle + KeystoneHalfWidthMetres, FlushWithTheWall, KeystoneProudMetres);
            surfaces.AddBlock(Surface.Dressing, keystone, head - KeystoneDropMetres, head + KeystoneRiseMetres);
        }
    }
}
