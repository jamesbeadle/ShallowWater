using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Houses
{
    public sealed class Plot
    {
        public Plot(GroundPoint frontCorner, GroundPoint lengthways, double lengthMetres, double depthMetres)
        {
            FrontCorner = frontCorner;
            Lengthways = lengthways;
            LengthMetres = lengthMetres;
            DepthMetres = depthMetres;
        }

        public GroundPoint FrontCorner { get; }
        public GroundPoint Lengthways { get; }
        public GroundPoint Backwards => Lengthways.RightAngleClockwise;
        public double LengthMetres { get; }
        public double DepthMetres { get; }

        public GroundPoint At(double along, double back)
        {
            return FrontCorner + Lengthways * along + Backwards * back;
        }

        public WorldPoint Point(double along, double back, double height)
        {
            var ground = At(along, back);
            return new WorldPoint(ground.East, height, ground.North);
        }

        public Plot Part(double along, double back, double lengthMetres, double depthMetres)
        {
            return new Plot(At(along, back), Lengthways, lengthMetres, depthMetres);
        }

        public WallFace Front(Rise rise)
        {
            return new WallFace(At(0, 0), At(LengthMetres, 0), rise);
        }

        public WallFace Back(Rise rise)
        {
            return new WallFace(At(LengthMetres, DepthMetres), At(0, DepthMetres), rise);
        }

        public WallFace LeftEnd(Rise rise)
        {
            return new WallFace(At(0, DepthMetres), At(0, 0), rise);
        }

        public WallFace RightEnd(Rise rise)
        {
            return new WallFace(At(LengthMetres, 0), At(LengthMetres, DepthMetres), rise);
        }

        public GroundRing Oblong(double along, double back, double halfLengthMetres, double halfDepthMetres)
        {
            return Footprints.Oblong(At(along, back), Lengthways, halfLengthMetres, halfDepthMetres);
        }
    }
}
