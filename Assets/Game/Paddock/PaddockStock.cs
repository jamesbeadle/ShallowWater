using System.Linq;
using ShallowWater.Game.Wildlife;

namespace ShallowWater.Game.Paddock
{
    public static class PaddockStock
    {
        private const int Seed = 1939;
        private const int Crows = 8;
        private const int Rooks = 2;

        public static Flock Stocked(PaddockShapes paddock)
        {
            var stations = paddock.Stations.ToList();
            var stockings = new[] { new Stocking(QuarryKind.Crow, Crows, stations), new Stocking(QuarryKind.Rook, Rooks, stations) };
            return Flock.Stocked(stockings, Seed);
        }
    }
}
