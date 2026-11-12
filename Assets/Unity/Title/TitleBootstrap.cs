using ShallowWater.Game.Saving;
using ShallowWater.Unity.Overlay;
using ShallowWater.Unity.Picture;
using ShallowWater.Unity.Saving;
using ShallowWater.Unity.World;
using UnityEngine;
using UnityEngine.UIElements;

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
            var backdrop = camera.gameObject.AddComponent<TitleBackdrop>();
            backdrop.Wear(screen.parent);
            new TitleScreen(screen, SaveFiles.Read(), start => WakeTheWorld(start, backdrop, screen));
        }

        private static void WakeTheWorld(GameStart start, TitleBackdrop backdrop, VisualElement screen)
        {
            Object.Destroy(backdrop);
            PoundBootstrap.Wake(start, screen);
        }
    }
}
