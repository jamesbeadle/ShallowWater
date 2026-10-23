using System.Collections.Generic;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Gatherings
    {
        public static GameObject Of(string name, IEnumerable<GameObject> parts)
        {
            var gathering = new GameObject(name);
            var parent = gathering.transform;
            foreach (var part in parts) Within(parent, part);
            return gathering;
        }

        public static void Within(Transform parent, GameObject part)
        {
            var placing = part.transform;
            placing.SetParent(parent, false);
        }
    }
}
