using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Fields;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;
using ShallowWater.Game.Woods;

namespace ShallowWater.Game.Paddock
{
    public sealed class PaddockShapes
    {
        private readonly List<GroundRing> footings = new List<GroundRing>();
        private readonly List<WorldPoint> perches = new List<WorldPoint>();

        private PaddockShapes(Paddock paddock)
        {
            Paddock = paddock;
        }

        public Paddock Paddock { get; }
        public SurfaceShapes Shapes { get; } = new SurfaceShapes();
        public IReadOnlyList<Hedge> Hedges { get; private set; } = new List<Hedge>();
        public IReadOnlyList<Tree> Trees { get; private set; } = new List<Tree>();
        public IReadOnlyList<GroundRing> Footings => footings;
        public IEnumerable<WorldPoint> Stations => PaddockPlan.FeedingPlaces(Paddock).Concat(perches);

        public static PaddockShapes Of(Paddock paddock)
        {
            var shapes = new PaddockShapes(paddock);
            shapes.SowThePasture();
            shapes.Hedges = PaddockHedges.Grow(shapes.Shapes, paddock);
            shapes.HangTheGate();
            shapes.StandTheTrough();
            shapes.PlantTheStandards();
            return shapes;
        }

        private void SowThePasture()
        {
            var corners = Paddock.Corners;
            var outline = new ConvexOutline(corners, Enumerable.Repeat(Boundary.FieldHedge, corners.Count));
            FieldShapes.Add(Shapes, new Field(outline, Crop.Pasture, Paddock.Lengthways));
        }

        private void HangTheGate()
        {
            var hinge = PaddockPlan.GateHinge(Paddock);
            var latch = PaddockPlan.GateLatch(Paddock);
            FieldGate.Add(Shapes, hinge, latch, PaddockPlan.GateSwingRadians);
            footings.AddRange(FieldGate.Footings(hinge, latch, PaddockPlan.GateSwingRadians));
            perches.Add(FieldGate.TopRailMiddle(hinge, latch, PaddockPlan.GateSwingRadians));
        }

        private void StandTheTrough()
        {
            var centre = PaddockPlan.TroughCentre(Paddock);
            Trough.Stand(Shapes, centre, Paddock.Lengthways);
            footings.Add(Trough.Footing(centre, Paddock.Lengthways));
            perches.Add(Trough.RimPerch(centre, Paddock.Lengthways));
        }

        private void PlantTheStandards()
        {
            Trees = PaddockStandards.Planted(Paddock);
            perches.AddRange(Trees.Select(PaddockStandards.CrownPerch));
        }
    }
}
