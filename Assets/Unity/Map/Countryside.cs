using System.Collections.Generic;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public sealed class Countryside
    {
        public Countryside(GameObject fields, IReadOnlyList<Tree> hedgerowTrees)
        {
            Fields = fields;
            HedgerowTrees = hedgerowTrees;
        }

        public GameObject Fields { get; }
        public IReadOnlyList<Tree> HedgerowTrees { get; }
    }
}
