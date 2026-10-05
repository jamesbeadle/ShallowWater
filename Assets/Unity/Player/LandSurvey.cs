using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Walking;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Map;

namespace ShallowWater.Unity.Player
{
    public static class LandSurvey
    {
        private const double Halved = 2;

        public static Land Of(Pound pound, IReadOnlyList<Tree> trees)
        {
            var water = new WaterLines();
            water.Add(pound.Line, pound.HalfWidth);
            foreach (var canal in MapLines.ReadIfPresent(MapLayers.Canal)) water.Add(canal, PoundLimits.ChannelHalfWidthMetres);
            foreach (var river in MapLines.ReadIfPresent(MapLayers.River)) water.Add(river, LineBands.RiverWidthMetres / Halved);
            var obstacles = new Obstacles();
            obstacles.AddFootprints(Buildings.Footings(MapAreas.ReadIfPresent(MapLayers.Buildings)));
            obstacles.AddTrunks(TreeTrunks.Standing(trees));
            return new Land(pound, water, obstacles, MapAreas.ReadIfPresent(MapLayers.Woods));
        }
    }
}
