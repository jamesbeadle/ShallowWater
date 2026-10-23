using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Paddock;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Roads;
using ShallowWater.Game.Walking;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.Woods;

namespace ShallowWater.Unity.World
{
    public sealed class Scenery
    {
        private Scenery(Countryside countryside, PrintedSheet periodMap, RoadNetwork roads, FarmyardShapes farm, PaddockShapes paddock, IReadOnlyList<Tree> trees)
        {
            Countryside = countryside;
            PeriodMap = periodMap;
            Roads = roads;
            Farm = farm;
            Paddock = paddock;
            Trees = trees;
        }

        public Countryside Countryside { get; }
        public PrintedSheet PeriodMap { get; }
        public RoadNetwork Roads { get; }
        public FarmyardShapes Farm { get; }
        public PaddockShapes Paddock { get; }
        public IReadOnlyList<Tree> Trees { get; }
        public Farmyard Yard => Farm.Yard;
        public IEnumerable<GroundRing> Footings => Farm.Footings.Concat(Paddock.Footings);
        public IEnumerable<Hedge> Hedges => Countryside.Hedges.Concat(Roads.Hedges).Concat(Paddock.Hedges);

        public Floors Floors
        {
            get
            {
                var floors = new Floors();
                var yard = Yard;
                var paddock = Paddock.Paddock;
                floors.AddSlab(yard.Outline, yard.FloorMetres);
                floors.AddSlab(paddock.Outline, paddock.FloorMetres);
                floors.AddStrips(Roads.Beds);
                return floors;
            }
        }

        public static Scenery Built(Pound pound)
        {
            var countryside = FieldsLayer.Lay(pound);
            var periodMap = GroundLayer.Lay(countryside.Fields);
            LandLinesLayer.Lay(MapLayers.River);
            LandLinesLayer.Lay(MapLayers.Railway);
            var roads = RoadsLayer.Lay(pound);
            VillagesLayer.Turf();
            BuildingsLayer.Raise();
            CanalLayer.Dig(pound);
            HuddlesfordPlanks.Drop(pound);
            GlascoteLock.CloseItsBottomGates(pound);
            FazeleyChain.Hang();
            var farm = DunstallFarm.Raise(pound);
            var paddock = HopwasPaddock.Raise(pound);
            var trees = WoodsLayer.Plant(countryside.Standards.Concat(paddock.Trees));
            return new Scenery(countryside, periodMap, roads, farm, paddock, trees);
        }
    }
}
