using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Picture;
using ShallowWater.Unity.Saving;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Title
{
    public static class TitleBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RaiseTheTitle()
        {
            var camera = HelmCamera.Main();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            var screen = OverlayScreen.Raise();
            new TitleScreen(screen, SaveFiles.Read(), start => PoundBootstrap.Wake(start, screen));
        }
    }
}
