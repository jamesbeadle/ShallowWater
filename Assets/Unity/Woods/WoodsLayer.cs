using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public static class WoodsLayer
    {
        private const string WoodsName = "Woods";
        private const string FloorName = "Woodland floor";

        public static IReadOnlyList<Tree> Plant(IEnumerable<Tree> hedgerowTrees)
        {
            var woods = MapAreas.ReadIfPresent(MapLayers.Woods);
            var floor = new Shape();
            foreach (var wood in woods) floor.Append(WoodFloor.Under(wood));
            ShapeMeshes.Build(FloorName, floor, Finishes.For(Surface.WoodlandFloor));
            var trees = woods.SelectMany(WoodPlanting.Within).Concat(hedgerowTrees).ToList();
            var renderer = new GameObject(WoodsName).AddComponent<WoodsRenderer>();
            renderer.Plant(trees);
            return trees;
        }
    }
}
