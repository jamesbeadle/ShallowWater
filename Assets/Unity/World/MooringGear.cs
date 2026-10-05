using ShallowWater.Game.Mooring;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class MooringGear
    {
        private const string LineName = "Stern line";
        private const string PinName = "Mooring pin";

        private GameObject line;
        private GameObject pin;

        public void Show(bool isLineMade, WorldPoint dolly, MooringPost post)
        {
            var isShown = line != null;
            if (isLineMade == isShown) return;
            if (isLineMade) Lay(dolly, post);
            if (!isLineMade) TakeUp();
        }

        private void Lay(WorldPoint dolly, MooringPost post)
        {
            line = Built(LineName, SternLine.Between(dolly, post.Tie));
            if (!post.IsPin) return;
            pin = Built(PinName, MooringPin.Knocked(post));
        }

        private void TakeUp()
        {
            Cleared(line);
            Cleared(pin);
            line = null;
            pin = null;
        }

        private static GameObject Built(string name, SurfaceShapes shapes)
        {
            var gear = new GameObject(name);
            foreach (var piece in ShapeMeshes.BuildEach(name, shapes)) piece.transform.SetParent(gear.transform, false);
            return gear;
        }

        private static void Cleared(GameObject gear)
        {
            if (gear == null) return;
            foreach (var filter in gear.GetComponentsInChildren<MeshFilter>()) Object.Destroy(filter.sharedMesh);
            Object.Destroy(gear);
        }
    }
}
