using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public readonly struct LaidLine
    {
        public LaidLine(MooringPost post, double lengthMetres)
        {
            Post = post;
            LengthMetres = lengthMetres;
        }

        public MooringPost Post { get; }
        public double LengthMetres { get; }
        public GroundPoint Anchor => Post.Place;
        public WorldPoint Tie => Post.Tie;
        public bool IsAtAPin => Post.IsPin;

        public static LaidLine TakenUpBetween(WorldPoint onTheBoat, MooringPost post)
        {
            var fixedEnd = new GroundPoint(onTheBoat.East, onTheBoat.North);
            return new LaidLine(post, fixedEnd.DistanceTo(post.Place));
        }
    }
}
