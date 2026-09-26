using System.Linq;
using ShallowWater.Game.Woods;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public static class WoodsLayer
    {
        public static void Plant()
        {
            if (!OptionalLayer.IsPresent(MapLayers.Woods)) return;
            var woods = MapAreas.Read(MapLayers.Woods);
            var trees = woods.SelectMany(TreeScatter.Within).ToList();
            var renderer = new GameObject("Woods").AddComponent<WoodsRenderer>();
            renderer.Plant(trees);
        }
    }
}
