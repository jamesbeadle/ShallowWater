using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class HuddlesfordPlanks
    {
        private const int PlankCount = 4;
        private const float PlankHeightMetres = 0.25f;
        private const float PlankThicknessMetres = 0.12f;
        private const float IntoEachBankMetres = 0.8f;
        private const float LowestPlankMetres = -0.5f;

        public static void Drop(Pound pound)
        {
            var paint = Finishes.For(Surface.Timber);
            var width = (float)pound.HalfWidth * 2 + IntoEachBankMetres * 2;
            var size = new Vector3(width, PlankHeightMetres, PlankThicknessMetres);
            var turn = PoundPlacement.AlongThePound(pound, pound.PlanksAlong);
            for (var plank = 0; plank < PlankCount; plank++)
            {
                var middle = LowestPlankMetres + PlankHeightMetres * plank + PlankHeightMetres / 2;
                var position = PoundPlacement.OnThePound(pound, pound.PlanksAlong, 0, middle);
                Blocks.Place(PrimitiveType.Cube, "Stop-plank", paint, new Placement(position, size, turn));
            }
        }
    }
}
