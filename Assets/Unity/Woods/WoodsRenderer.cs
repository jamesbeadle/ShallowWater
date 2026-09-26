using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class WoodsRenderer : MonoBehaviour
    {
        private readonly List<TreeBatch> batches = new List<TreeBatch>();

        public void Plant(IReadOnlyList<Tree> trees)
        {
            var trunks = TreePart.Trunk();
            var crowns = TreePart.Crown();
            for (var first = 0; first < trees.Count; first += TreeBatch.MostTrees)
            {
                var batch = trees.Skip(first).Take(TreeBatch.MostTrees).ToList();
                batches.Add(new TreeBatch(batch, trunks, crowns));
            }
        }

        private void Update()
        {
            foreach (var batch in batches) batch.Draw();
        }
    }
}
