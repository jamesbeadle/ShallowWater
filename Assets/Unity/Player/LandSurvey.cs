using System.Collections.Generic;
using System.Linq;
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
            foreach (var canal in LinesOf(MapLayers.Canal)) water.Add(canal, PoundLimits.ChannelHalfWidthMetres);
            foreach (var river in LinesOf(MapLayers.River)) water.Add(river, LineBands.RiverWidthMetres / Halved);
            var obstacles = new Obstacles();
            obstacles.AddFootprints(AreasOf(MapLayers.Buildings).Select(building => building.Outline));
            obstacles.AddTrunks(TreeTrunks.Standing(trees));
            return new Land(pound, water, obstacles, AreasOf(MapLayers.Woods));
        }

        private static IReadOnlyList<GroundLine> LinesOf(string layerName)
        {
            var isPresent = OptionalLayer.IsPresent(layerName);
            return isPresent ? MapLines.Read(layerName) : new List<GroundLine>();
        }

        private static IReadOnlyList<Area> AreasOf(string layerName)
        {
            var isPresent = OptionalLayer.IsPresent(layerName);
            return isPresent ? MapAreas.Read(layerName) : new List<Area>();
        }
    }
}
