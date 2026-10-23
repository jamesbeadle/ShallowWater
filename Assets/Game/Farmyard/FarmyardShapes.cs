using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Farmyard
{
    public sealed class FarmyardShapes
    {
        private const int HeapSides = 12;
        private const double HeapDepthShare = 0.8;
        private const double OnTheFloor = 0;
        private static readonly Offset MuckHeapShape = new Offset(
            FarmyardPlan.MuckHeapRadiusMetres, FarmyardPlan.MuckHeapHeightMetres, FarmyardPlan.MuckHeapRadiusMetres * HeapDepthShare);

        private readonly PostAndRail fence;

        private FarmyardShapes(Farmyard yard)
        {
            Yard = yard;
            fence = new PostAndRail(yard);
        }

        public Farmyard Yard { get; }
        public SurfaceShapes Shapes { get; } = new SurfaceShapes();
        public IEnumerable<GroundRing> Footings => fence.Footings.Concat(FarmyardPlan.Mounds(Yard));

        public IEnumerable<WorldPoint> Perches
        {
            get
            {
                var rickTops = FarmyardPlan.RickFeet(Yard).Select(foot => Yard.Raised(foot, FarmyardPlan.RickApexMetres));
                var heapTop = Yard.Raised(FarmyardPlan.MuckHeapFoot(Yard), FarmyardPlan.MuckHeapHeightMetres);
                return fence.PostTops.Concat(rickTops).Append(heapTop);
            }
        }

        public static FarmyardShapes Of(Farmyard yard)
        {
            var farmyard = new FarmyardShapes(yard);
            farmyard.Floor();
            farmyard.fence.Fence(farmyard.Shapes);
            foreach (var foot in FarmyardPlan.RickFeet(yard)) Hayrick.Stand(farmyard.Shapes, yard.Raised(foot, OnTheFloor));
            farmyard.MuckHeap();
            return farmyard;
        }

        private void Floor()
        {
            Shapes.Add(Surface.Lane, Extrusion.Roof(Yard.Outline, Yard.FloorMetres));
        }

        private void MuckHeap()
        {
            var foot = Yard.Raised(FarmyardPlan.MuckHeapFoot(Yard), OnTheFloor);
            Shapes.Add(Surface.Bank, Ball.Shaped(foot, MuckHeapShape, HeapSides));
        }
    }
}
