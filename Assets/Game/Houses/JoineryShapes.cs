using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class JoineryShapes
    {
        private const double WindowSetBackMetres = 0.1;
        private const double DoorSetBackMetres = 0.13;

        public static void Fit(SurfaceShapes surfaces, WallFace face, IReadOnlyList<Joinery> joinery, Surface walls, Lintel lintel)
        {
            var openings = joinery.Select(piece => piece.Opening).ToList();
            surfaces.Add(walls, PiercedWall.Around(face, openings));
            foreach (var piece in joinery) FitOne(surfaces, face, piece, walls, lintel);
        }

        private static void FitOne(SurfaceShapes surfaces, WallFace face, Joinery piece, Surface walls, Lintel lintel)
        {
            var setBack = piece.IsDoor ? DoorSetBackMetres : WindowSetBackMetres;
            var filling = piece.IsDoor ? Surface.Door : Surface.Window;
            surfaces.Add(walls, Recess.Reveals(face, piece.Opening, setBack));
            surfaces.Add(filling, Recess.Filling(face, piece.Opening, setBack, piece.Fitting));
            Dressings.Add(surfaces, face, piece, setBack);
            Lintels.Add(surfaces, face, piece.Opening, LintelFinish(walls), lintel);
        }

        private static Surface LintelFinish(Surface walls)
        {
            var isLimewashed = walls == Surface.Limewash;
            return isLimewashed ? Surface.Limewash : Surface.Voussoirs;
        }
    }
}
