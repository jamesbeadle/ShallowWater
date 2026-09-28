using System;
using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Woods
{
    public static class SpeciesMix
    {
        private const double PatchMetres = 90;
        private const int BirchSalt = 7;
        private const int AshSalt = 13;
        private const double PatchFrom = 0.55;
        private const double PatchTo = 0.8;
        private const double PatchShare = 0.6;
        private const double ScatteredBirch = 0.07;
        private const double ScatteredAsh = 0.08;
        private const double EdgeBirch = 0.2;
        private const double EdgeMetres = 16;
        private const double YoungShare = 0.22;

        public static Habit HabitAt(GroundPoint place, double edgeMetres, Random random)
        {
            var isYoung = random.NextDouble() < YoungShare;
            var isAtTheEdge = edgeMetres < EdgeMetres;
            var species = SpeciesAt(place, isAtTheEdge, random);
            if (species == TreeSpecies.Birch) return isYoung ? BirchHabits.Young : BirchHabits.Mature;
            if (species == TreeSpecies.Ash) return isYoung ? AshHabits.Young : AshHabits.Woodland;
            if (isAtTheEdge) return OakHabits.OpenGrown;
            return isYoung ? OakHabits.Young : OakHabits.Woodland;
        }

        private static TreeSpecies SpeciesAt(GroundPoint place, bool isAtTheEdge, Random random)
        {
            var birchShare = ScatteredBirch + Patch(place, BirchSalt) + (isAtTheEdge ? EdgeBirch : 0);
            var ashShare = ScatteredAsh + Patch(place, AshSalt);
            var roll = random.NextDouble();
            if (roll < birchShare) return TreeSpecies.Birch;
            if (roll < birchShare + ashShare) return TreeSpecies.Ash;
            return TreeSpecies.Oak;
        }

        private static double Patch(GroundPoint place, int salt)
        {
            var noise = WoodNoise.At(place, PatchMetres, salt);
            var share = Math.Min(Math.Max((noise - PatchFrom) / (PatchTo - PatchFrom), 0), 1);
            return share * PatchShare;
        }
    }
}
