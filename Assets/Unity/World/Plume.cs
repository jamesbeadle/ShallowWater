using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.World
{
    public static class Plume
    {
        private const string PlumeName = "Smoke";
        private static readonly Bounds Reach = new Bounds(Vector3.up * 4f, Vector3.one * 16f);

        public static void Rising(Transform parent, Vector3 localPlace, Material look, int puffs)
        {
            var plume = new GameObject(PlumeName);
            var placement = plume.transform;
            placement.SetParent(parent, false);
            placement.localPosition = localPlace;
            var filter = plume.AddComponent<MeshFilter>();
            filter.sharedMesh = QuadCloud.Made(puffs, Reach);
            var renderer = plume.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = look;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }
    }
}
