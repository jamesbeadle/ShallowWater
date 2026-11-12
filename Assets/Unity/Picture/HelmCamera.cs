using UnityEngine;

namespace ShallowWater.Unity.Picture
{
    public static class HelmCamera
    {
        private const string CameraName = "Helm camera";
        private const string MainCameraTag = "MainCamera";

        public static Camera Main()
        {
            var camera = Camera.main;
            var hasACamera = camera != null;
            if (hasACamera) return camera;
            camera = new GameObject(CameraName).AddComponent<Camera>();
            camera.tag = MainCameraTag;
            return camera;
        }
    }
}
