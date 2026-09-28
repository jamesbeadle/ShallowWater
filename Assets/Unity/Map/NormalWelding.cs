using System.Collections.Generic;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class NormalWelding
    {
        private const float SamePlaceMetres = 1e-4f;

        public static Vector3[] Welded(Vector3[] vertices, Vector3[] normals)
        {
            var sums = new Dictionary<Vector3Int, Vector3>();
            for (var index = 0; index < vertices.Length; index++)
            {
                var place = PlaceOf(vertices[index]);
                sums.TryGetValue(place, out var sum);
                sums[place] = sum + normals[index];
            }
            var welded = new Vector3[vertices.Length];
            for (var index = 0; index < vertices.Length; index++) welded[index] = sums[PlaceOf(vertices[index])].normalized;
            return welded;
        }

        private static Vector3Int PlaceOf(Vector3 vertex)
        {
            return Vector3Int.RoundToInt(vertex / SamePlaceMetres);
        }
    }
}
