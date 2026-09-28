using UnityEngine;

namespace ShallowWater.Unity.Picture
{
    public static class HelmPicture
    {
        private const float FieldOfViewDegrees = 50f;
        private const float NearestMetres = 0.3f;
        private const float FarthestMetres = 3000f;

        public static void Frame(Camera camera)
        {
            camera.allowHDR = true;
            camera.allowMSAA = true;
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.fieldOfView = FieldOfViewDegrees;
            camera.nearClipPlane = NearestMetres;
            camera.farClipPlane = FarthestMetres;
            camera.gameObject.AddComponent<PictureGrade>();
        }
    }
}
