using System;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Mooring
{
    public static class HangingLine
    {
        public const double RadiusMetres = 0.012;
        private const double TakenUpSagShare = 0.08;
        private const int Spans = 14;
        private const int RopeSides = 6;
        private const double ParabolaDepth = 4;
        private const double ArcOverTheChord = 8.0 / 3;
        private const double Half = 0.5;
        private const double Taut = 0;

        public static Shape TakenUp(WorldPoint from, WorldPoint to)
        {
            var reach = Offset.Between(from, to);
            return Between(from, to, TakenUpSagShare * reach.Length);
        }

        public static Shape WithSlack(WorldPoint from, WorldPoint to, double slackMetres)
        {
            var chord = Offset.Between(from, to).Length;
            var slack = Math.Max(Taut, slackMetres);
            var sag = Math.Sqrt(chord * slack / ArcOverTheChord);
            var aboveTheWater = (from.Height + to.Height) * Half - Heights.WaterMetres - RadiusMetres;
            return Between(from, to, Math.Min(sag, Math.Max(Taut, aboveTheWater)));
        }

        public static Shape Between(WorldPoint from, WorldPoint to, double sagMetres)
        {
            var reach = Offset.Between(from, to);
            var hanging = Enumerable.Range(0, Spans + 1).Select(span =>
            {
                var share = (double)span / Spans;
                var drop = ParabolaDepth * sagMetres * share * (1 - share);
                return (reach * share + Offset.Up * -drop).From(from);
            });
            return Tube.Along(hanging.ToList(), RadiusMetres, RopeSides);
        }
    }
}
