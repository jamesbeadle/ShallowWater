using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class MooringLineShape
    {
        private const double InHandSlackMetres = 0;
        private const double MadeFastSlackMetres = 0.1;

        public static Shape InHand(LineEnd stud, WorldPoint hand, SurfaceBeneath beneath)
        {
            var turns = Turns.UpTheStud(stud, hand.Place);
            return Rope.Laid(Draped(turns, hand, CentreLine.LengthMetres, InHandSlackMetres, beneath));
        }

        public static Shape MadeFast(LineEnd stud, LaidLine laid, SurfaceBeneath beneath)
        {
            var post = laid.Post;
            var turns = Turns.UpTheStud(stud, post.Place);
            var onThePost = Turns.DownThePost(post, stud.Place);
            var rope = Draped(turns, onThePost[0], laid.LengthMetres, MadeFastSlackMetres, beneath);
            rope.AddRange(onThePost.Skip(1));
            return Rope.Laid(rope);
        }

        private static List<WorldPoint> Draped(List<WorldPoint> turns, WorldPoint end, double lengthMetres, double leastSlackMetres, SurfaceBeneath beneath)
        {
            var start = turns[turns.Count - 1];
            var lead = new Lead(start.Place, end.Place, beneath);
            var draped = DrapedLine.Over(lead, start.Height, end.Height, lengthMetres, leastSlackMetres);
            var rope = new List<WorldPoint>(turns);
            rope.AddRange(draped.Skip(1));
            return rope;
        }
    }
}
