using ShallowWater.Unity.Looks;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShallowWater.Unity.World
{
    public static class Drizzle
    {
        private const string DrizzleName = "Drizzle";
        private static readonly Bounds Reach = new Bounds(Vector3.zero, DrizzleLook.Box * 2f);

        public static void Falling(Camera eye)
        {
            var drizzle = new GameObject(DrizzleName);
            drizzle.transform.SetParent(eye.transform, false);
            var filter = drizzle.AddComponent<MeshFilter>();
            filter.sharedMesh = QuadCloud.Made(DrizzleLook.Streaks, Reach);
            var renderer = drizzle.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = DrizzleLook.October();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }
    }
}
