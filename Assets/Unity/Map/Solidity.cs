using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public static class Solidity
    {
        public static List<GameObject> Hardened(IEnumerable<GameObject> built)
        {
            var hardened = built.ToList();
            foreach (var piece in hardened) Harden(piece);
            return hardened;
        }

        public static GameObject Harden(GameObject piece)
        {
            var filter = piece.GetComponent<MeshFilter>();
            var mesh = filter.sharedMesh;
            var isEmpty = mesh.vertexCount == 0;
            if (isEmpty) return piece;
            var collider = piece.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            return piece;
        }
    }
}
