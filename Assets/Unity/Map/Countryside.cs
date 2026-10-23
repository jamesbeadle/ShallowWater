using System.Collections.Generic;
using ShallowWater.Game.Walking;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public sealed class Countryside
    {
        public Countryside(GameObject fields, IReadOnlyList<Tree> standards, IReadOnlyList<Hedge> hedges)
        {
            Fields = fields;
            Standards = standards;
            Hedges = hedges;
        }

        public GameObject Fields { get; }
        public IReadOnlyList<Tree> Standards { get; }
        public IReadOnlyList<Hedge> Hedges { get; }
    }
}
