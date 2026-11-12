using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class InnSign
    {
        private const double PostOutFromTheWallMetres = 0.9;
        private const double PostInFromTheCornerMetres = 0.7;
        private const double PostSideMetres = 0.18;
        private const double PostHeightMetres = 4.5;
        private const double ArmReachMetres = 1.25;
        private const double ArmDepthMetres = 0.12;
        private const double ArmHeightMetres = 4.1;
        private const double BoardWidthMetres = 0.95;
        private const double BoardThicknessMetres = 0.06;
        private const double BoardTopMetres = 3.85;
        private const double BoardBottomMetres = 2.65;
        private const double HangerSideMetres = 0.025;
        private const double HangerInsetMetres = 0.12;
        private const double Half = 0.5;

        public static void Hang(SurfaceShapes surfaces, Plot inn)
        {
            var outwards = inn.Backwards * -1;
            var foot = inn.At(PostInFromTheCornerMetres, -PostOutFromTheWallMetres);
            Box(surfaces, Surface.Timber, foot, outwards, PostSideMetres, PostSideMetres, 0, PostHeightMetres);
            var armMiddle = foot + outwards * (ArmReachMetres * Half);
            Box(surfaces, Surface.Timber, armMiddle, outwards, ArmReachMetres, ArmDepthMetres, ArmHeightMetres - ArmDepthMetres, ArmHeightMetres);
            var boardMiddle = foot + outwards * (PostSideMetres + BoardWidthMetres * Half);
            Box(surfaces, Surface.SignBoard, boardMiddle, outwards, BoardWidthMetres, BoardThicknessMetres, BoardBottomMetres, BoardTopMetres);
            Hangers(surfaces, boardMiddle, outwards);
        }

        private static void Hangers(SurfaceShapes surfaces, GroundPoint boardMiddle, GroundPoint outwards)
        {
            var reach = BoardWidthMetres * Half - HangerInsetMetres;
            foreach (var side in new[] { -reach, reach })
            {
                var hanger = boardMiddle + outwards * side;
                Box(surfaces, Surface.Iron, hanger, outwards, HangerSideMetres, HangerSideMetres, BoardTopMetres, ArmHeightMetres - ArmDepthMetres);
            }
        }

        private static void Box(SurfaceShapes surfaces, Surface surface, GroundPoint centre, GroundPoint along, double lengthMetres, double widthMetres, double bottomMetres, double topMetres)
        {
            var footprint = Footprints.Oblong(centre, along, lengthMetres * Half, widthMetres * Half);
            surfaces.AddSolid(surface, footprint, Heights.GroundMetres + bottomMetres, Heights.GroundMetres + topMetres);
        }
    }
}
