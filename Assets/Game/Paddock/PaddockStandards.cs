using System;
using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Paddock
{
    public static class PaddockStandards
    {
        private const int Seed = 1940;
        private const double Scale = 1.1;
        private const double CrownPerchShare = 0.8;
        private const double FullTurnRadians = 2 * Math.PI;

        public static List<Tree> Planted(Paddock paddock)
        {
            var random = new Random(Seed);
            return PaddockPlan.StandardFeet(paddock).Select(foot => Standard(foot, random)).ToList();
        }

        public static WorldPoint CrownPerch(Tree tree)
        {
            var form = TreeForms.All[tree.Form];
            var habit = form.Habit;
            return new WorldPoint(tree.East, Heights.GroundMetres + habit.HeightMetres * tree.Scale * CrownPerchShare, tree.North);
        }

        private static Tree Standard(GroundPoint foot, Random random)
        {
            return new Tree(foot, Scale, random.NextDouble() * FullTurnRadians, TreeForms.OneOf(OakHabits.OpenGrown, random));
        }
    }
}
