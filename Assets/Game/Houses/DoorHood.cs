using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public static class DoorHood
    {
        private const double BeyondTheDoorMetres = 0.24;
        private const double AboveTheHeadMetres = 0.34;
        private const double ThicknessMetres = 0.09;
        private const double ProjectionMetres = 0.45;
        private const double BracketWidthMetres = 0.07;
        private const double BracketInFromTheEndMetres = 0.06;
        private const double BracketProjectionMetres = 0.34;
        private const double BracketDropMetres = 0.36;
        private const double FlushWithTheWall = 0;

        public static void Hang(SurfaceShapes surfaces, WallFace front, Opening door)
        {
            var rise = front.Rise;
            var bottom = rise.FootMetres + door.Top + AboveTheHeadMetres;
            var left = door.Left - BeyondTheDoorMetres;
            var right = door.Right + BeyondTheDoorMetres;
            surfaces.AddBlock(Surface.Timber, front.Slab(left, right, FlushWithTheWall, ProjectionMetres), bottom, bottom + ThicknessMetres);
            var leftBracket = left + BracketInFromTheEndMetres;
            var rightBracket = right - BracketInFromTheEndMetres;
            var leftFoot = front.Slab(leftBracket, leftBracket + BracketWidthMetres, FlushWithTheWall, BracketProjectionMetres);
            var rightFoot = front.Slab(rightBracket - BracketWidthMetres, rightBracket, FlushWithTheWall, BracketProjectionMetres);
            surfaces.AddBlock(Surface.Timber, leftFoot, bottom - BracketDropMetres, bottom);
            surfaces.AddBlock(Surface.Timber, rightFoot, bottom - BracketDropMetres, bottom);
        }
    }
}
