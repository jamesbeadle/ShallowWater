using System.Collections.Generic;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class WoodsRenderer : MonoBehaviour
    {
        private readonly Dictionary<Camera, WoodView> views = new Dictionary<Camera, WoodView>();
        private List<WoodTile> tiles = new List<WoodTile>();

        public void Plant(IReadOnlyList<Tree> trees)
        {
            tiles = WoodTiles.Laid(trees, TreeModels.Made());
            views.Clear();
        }

        private void OnEnable()
        {
            Camera.onPreCull += DrawFor;
        }

        private void OnDisable()
        {
            Camera.onPreCull -= DrawFor;
        }

        private void DrawFor(Camera camera)
        {
            var kind = camera.cameraType;
            var isLookingAtTheWorld = kind == CameraType.Game || kind == CameraType.SceneView;
            if (!isLookingAtTheWorld) return;
            var hasView = views.TryGetValue(camera, out var view);
            if (!hasView) view = views[camera] = new WoodView(tiles);
            view.DrawFor(camera);
        }
    }
}
