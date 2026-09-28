using System.Collections.Generic;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class WoodView
    {
        private const float RebinMetres = 8f;
        private static readonly TreePlacements[] Nothing = new TreePlacements[0];
        private readonly IReadOnlyList<WoodTile> tiles;
        private readonly IReadOnlyList<TreePlacements>[] shown;
        private Vector3 binnedFrom;
        private bool hasBinned;

        public WoodView(IReadOnlyList<WoodTile> tiles)
        {
            this.tiles = tiles;
            shown = new IReadOnlyList<TreePlacements>[tiles.Count];
        }

        public void DrawFor(Camera camera)
        {
            var viewpoint = camera.transform;
            var eye = viewpoint.position;
            var hasMoved = !hasBinned || Vector3.Distance(eye, binnedFrom) > RebinMetres;
            if (hasMoved) Rebin(eye);
            for (var tile = 0; tile < tiles.Count; tile++)
            {
                var bounds = tiles[tile].Bounds;
                foreach (var placements in shown[tile]) placements.Draw(bounds, camera);
            }
        }

        private void Rebin(Vector3 eye)
        {
            for (var tile = 0; tile < tiles.Count; tile++) shown[tile] = Shown(tiles[tile], eye);
            binnedFrom = eye;
            hasBinned = true;
        }

        private static IReadOnlyList<TreePlacements> Shown(WoodTile tile, Vector3 eye)
        {
            if (!tile.IsWithinSightOf(eye)) return Nothing;
            return tile.IsWithinDetailOf(eye) ? tile.SeenFrom(eye) : tile.FarAway;
        }
    }
}
