using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.World;

namespace ShallowWater.Unity.Player
{
    public static class LandSurvey
    {
        private const double Halved = 2;

        public static Land Of(Pound pound, Scenery scenery)
        {
            var water = new WaterLines();
            water.Add(pound.Line, pound.HalfWidth);
            foreach (var canal in MapLines.ReadIfPresent(MapLayers.Canal)) water.Add(canal, PoundLimits.ChannelHalfWidthMetres);
            foreach (var river in MapLines.ReadIfPresent(MapLayers.River)) water.Add(river, LineBands.RiverWidthMetres / Halved);
            var obstacles = new Obstacles();
            obstacles.AddFootprints(Buildings.Footings(MapAreas.ReadIfPresent(MapLayers.Buildings)));
            obstacles.AddTrunks(TreeTrunks.Standing(scenery.Trees));
            obstacles.AddFootprints(scenery.Footings);
            obstacles.AddHedges(scenery.Hedges);
            var relief = new Relief(MapAreas.ReadIfPresent(MapLayers.Woods), MapAreas.ReadIfPresent(MapLayers.Villages), scenery.Floors);
            return new Land(pound, water, obstacles, relief);
        }
    }
}
