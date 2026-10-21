using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Farmyard
{
    public sealed class FarmyardShapes
    {
        private const double FloorAboveTheGroundMetres = 0.02;
        private const int HeapSides = 12;
        private const double HeapDepthShare = 0.8;
        private static readonly Offset MuckHeapShape = new Offset(
            FarmyardPlan.MuckHeapRadiusMetres, FarmyardPlan.MuckHeapHeightMetres, FarmyardPlan.MuckHeapRadiusMetres * HeapDepthShare);

        private readonly Farmyard yard;
        private readonly PostAndRail fence;

        private FarmyardShapes(Farmyard yard)
        {
            this.yard = yard;
            fence = new PostAndRail(yard);
        }

        public SurfaceShapes Shapes { get; } = new SurfaceShapes();
        public IReadOnlyList<GroundRing> Footings => fence.Footings;

        public IEnumerable<WorldPoint> Perches
        {
            get
            {
                var rickTops = FarmyardPlan.RickFeet(yard).Select(foot => yard.Raised(foot, FarmyardPlan.RickApexMetres));
                var heapTop = yard.Raised(FarmyardPlan.MuckHeapFoot(yard), FarmyardPlan.MuckHeapHeightMetres);
                return fence.PostTops.Concat(rickTops).Append(heapTop);
            }
        }

        public static FarmyardShapes Of(Farmyard yard)
        {
            var farmyard = new FarmyardShapes(yard);
            farmyard.Floor();
            farmyard.fence.Fence(farmyard.Shapes);
            foreach (var foot in FarmyardPlan.RickFeet(yard)) Hayrick.Stand(farmyard.Shapes, yard.Raised(foot, 0));
            farmyard.MuckHeap();
            return farmyard;
        }

        private void Floor()
        {
            Shapes.Add(Surface.Lane, Extrusion.Roof(yard.Outline, yard.FloorMetres + FloorAboveTheGroundMetres));
        }

        private void MuckHeap()
        {
            var foot = yard.Raised(FarmyardPlan.MuckHeapFoot(yard), 0);
            Shapes.Add(Surface.Bank, Ball.Shaped(foot, MuckHeapShape, HeapSides));
        }
    }
}
