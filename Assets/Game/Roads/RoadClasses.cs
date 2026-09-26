using System;
using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Roads
{
    public static class RoadClasses
    {
        private const double MainRoadWidthMetres = 7;
        private const double MainRoadVergeMetres = 2;
        private const double MainRoadHeightMetres = -0.16;
        private const double SecondaryRoadWidthMetres = 6;
        private const double SecondaryRoadVergeMetres = 1.5;
        private const double SecondaryRoadHeightMetres = -0.17;
        private const double MinorRoadWidthMetres = 5;
        private const double MinorRoadVergeMetres = 1.2;
        private const double MinorRoadHeightMetres = -0.18;
        private const double LaneWidthMetres = 3.6;
        private const double LaneVergeMetres = 0.8;
        private const double LaneHeightMetres = -0.19;

        public static readonly RoadClass Main =
            new RoadClass(Surface.MainRoad, MainRoadWidthMetres, MainRoadVergeMetres, MainRoadHeightMetres, Furniture.CentreLineAndTelegraphPoles);
        public static readonly RoadClass Secondary =
            new RoadClass(Surface.MainRoad, SecondaryRoadWidthMetres, SecondaryRoadVergeMetres, SecondaryRoadHeightMetres, Furniture.CentreLine);
        public static readonly RoadClass Minor =
            new RoadClass(Surface.Road, MinorRoadWidthMetres, MinorRoadVergeMetres, MinorRoadHeightMetres, Furniture.None);
        public static readonly RoadClass Lane =
            new RoadClass(Surface.Lane, LaneWidthMetres, LaneVergeMetres, LaneHeightMetres, Furniture.None);

        private static readonly Dictionary<string, RoadClass> ByKind = new Dictionary<string, RoadClass>
        {
            { LineKinds.PrimaryRoad, Main },
            { LineKinds.SecondaryRoad, Secondary },
            { LineKinds.TertiaryRoad, Minor },
            { LineKinds.UnclassifiedRoad, Lane },
        };

        public static RoadClass For(string kind)
        {
            var isKnown = ByKind.TryGetValue(kind, out var roadClass);
            if (isKnown) return roadClass;
            throw new ArgumentException($"No road class is known for a road of kind '{kind}'.", nameof(kind));
        }
    }
}
