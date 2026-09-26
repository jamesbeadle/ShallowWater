using ShallowWater.Game.Pound;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class HuddlesfordPlanks
    {
        private static readonly Color TarredOak = new Color(0.22f, 0.16f, 0.11f);
        private const int PlankCount = 4;
        private const float PlankHeightMetres = 0.25f;
        private const float PlankThicknessMetres = 0.12f;
        private const float IntoEachBankMetres = 0.8f;
        private const float LowestPlankMetres = -0.5f;

        public static void Drop(Pound pound)
        {
            var paint = Paint.Of(TarredOak);
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
