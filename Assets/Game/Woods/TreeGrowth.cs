using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Woods
{
    public static class TreeGrowth
    {
        private const double FootBelowTheGroundMetres = 0.2;
        private const double FullTurnRadians = 2 * Math.PI;
        private const double DegreesToRadians = Math.PI / 180;
        private const double ForkShareUpTheTrunk = 0.93;
        private const double ForkJitter = 0.15;
        private const double ShorterTowardsTheTip = 0.5;

        public static IReadOnlyList<Limb> Grown(Habit habit, int seed)
        {
            var random = new Random(seed);
            var trunk = TrunkFlare.Flared(LimbGrowth.Grown(TrunkStart(habit, random), habit.Trunk, 0, random), habit.FlareShare);
            var limbs = new List<Limb> { trunk };
            var level = habit.Levels[0];
            var crownBase = level.FromShare;
            Func<double, double> byTheCrown = share => CrownOutlines.LengthShareAt(habit.Outline, (share - crownBase) / (1 - crownBase));
            var starts = ForkLimbs(trunk, habit, random).Concat(Offshoots.Along(trunk, level, byTheCrown, random)).ToList();
            foreach (var start in starts) GrowOn(limbs, start, 1, habit, random);
            return limbs;
        }

        private static void GrowOn(List<Limb> limbs, LimbStart start, int order, Habit habit, Random random)
        {
            var limb = LimbGrowth.Grown(start, habit.Levels[order - 1], order, random);
            limbs.Add(limb);
            var isLeafBearing = order == habit.LeafOrder;
            if (isLeafBearing) return;
            var offshoots = Offshoots.Along(limb, habit.Levels[order], share => 1 - share * ShorterTowardsTheTip, random);
            foreach (var offshoot in offshoots) GrowOn(limbs, offshoot, order + 1, habit, random);
        }

        private static LimbStart TrunkStart(Habit habit, Random random)
        {
            var foot = new WorldPoint(0, -FootBelowTheGroundMetres, 0);
            var side = Offset.East.TurnedAbout(Offset.Up, random.NextDouble() * FullTurnRadians);
            var lean = habit.LeanRadians * random.NextDouble();
            var heading = Offset.Up * Math.Cos(lean) + side * Math.Sin(lean);
            var length = habit.HeightMetres * habit.TrunkShare + FootBelowTheGroundMetres;
            return new LimbStart(foot, heading, length, habit.TrunkRadiusMetres);
        }

        private static IEnumerable<LimbStart> ForkLimbs(Limb trunk, Habit habit, Random random)
        {
            var fork = trunk.At(ForkShareUpTheTrunk);
            for (var limb = 0; limb < habit.ForkLimbs; limb++)
            {
                var around = FullTurnRadians * (limb + Jitter(random) * ForkJitter) / habit.ForkLimbs;
                var side = Offset.East.TurnedAbout(Offset.Up, around);
                var angle = habit.ForkAngleDegrees * DegreesToRadians * (1 + Jitter(random) * ForkJitter);
                var heading = fork.Heading * Math.Cos(angle) + side * Math.Sin(angle);
                var length = habit.HeightMetres * habit.ForkLengthShare * (1 + Jitter(random) * ForkJitter);
                yield return new LimbStart(fork.Place, heading, length, fork.RadiusMetres * habit.ForkRadiusShare);
            }
        }

        private static double Jitter(Random random)
        {
            return random.NextDouble() * 2 - 1;
        }
    }
}
