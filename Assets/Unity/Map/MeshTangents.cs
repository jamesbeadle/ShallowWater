using System.Collections.Generic;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class MeshTangents
    {
        private const float NearlyUpright = 0.95f;
        private const float RightHanded = 1f;

        public static List<Vector4> Across(Vector3[] normals)
        {
            var tangents = new List<Vector4>(normals.Length);
            foreach (var normal in normals) tangents.Add(Across(normal));
            return tangents;
        }

        private static Vector4 Across(Vector3 normal)
        {
            var isUpright = Mathf.Abs(normal.y) > NearlyUpright;
            var reference = isUpright ? Vector3.right : Vector3.up;
            var across = Vector3.Cross(reference, normal);
            across.Normalize();
            return new Vector4(across.x, across.y, across.z, RightHanded);
        }
    }
}
