using System.Linq;
using ShallowWater.Game.Wildlife;

namespace ShallowWater.Game.Farmyard
{
    public static class FarmyardStock
    {
        private const int Seed = 1938;
        private const int Rooks = 3;
        private const int Crows = 2;
        private const int Magpies = 1;
        private const int Rats = 4;
        private const double OnTheFloor = 0;

        public static Flock Stocked(FarmyardShapes farm)
        {
            var yard = farm.Yard;
            var perches = farm.Perches.ToList();
            var ratRuns = FarmyardPlan.RatRuns(yard).Select(run => yard.Raised(run, OnTheFloor)).ToList();
            var stockings = new[]
            {
                new Stocking(QuarryKind.Rook, Rooks, perches), new Stocking(QuarryKind.Crow, Crows, perches),
                new Stocking(QuarryKind.Magpie, Magpies, perches), new Stocking(QuarryKind.Rat, Rats, ratRuns)
            };
            return Flock.Stocked(stockings, Seed);
        }
    }
}
