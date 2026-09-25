using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreeBatch
    {
        public const int MostTrees = 1023;
        private const float TallestTreeMetres = 14f;

        private readonly TreePart trunks;
        private readonly TreePart crowns;
        private readonly Matrix4x4[] trunkPlacements;
        private readonly Matrix4x4[] crownPlacements;
        private readonly Bounds bounds;

        public TreeBatch(IReadOnlyList<Tree> trees, TreePart trunks, TreePart crowns)
        {
            this.trunks = trunks;
            this.crowns = crowns;
            trunkPlacements = trees.Select(trunks.PlacedAt).ToArray();
            crownPlacements = trees.Select(crowns.PlacedAt).ToArray();
            bounds = BoundsAround(trees);
        }

        public void Draw()
        {
            trunks.Draw(trunkPlacements, bounds);
            crowns.Draw(crownPlacements, bounds);
        }

        private static Bounds BoundsAround(IReadOnlyList<Tree> trees)
        {
            var west = trees.Min(tree => tree.East);
            var east = trees.Max(tree => tree.East);
            var south = trees.Min(tree => tree.North);
            var north = trees.Max(tree => tree.North);
            var centre = new Vector3((float)(west + east) / 2, TallestTreeMetres / 2, (float)(south + north) / 2);
            var reach = new Vector3((float)(east - west), TallestTreeMetres, (float)(north - south));
            return new Bounds(centre, reach + Vector3.one * TallestTreeMetres);
        }
    }
}
