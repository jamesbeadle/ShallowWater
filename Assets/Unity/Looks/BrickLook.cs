using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class BrickLook
    {
        private const float Yes = 1;
        private const float No = 0;
        private const float FrontWindowsFromTheMiddleMetres = 2.1f;
        private const float GableWindowsFromTheMiddleMetres = 0;
        private static readonly int Frame = Shader.PropertyToID("_Frame");
        private static readonly int Glass = Shader.PropertyToID("_Glass");
        private static readonly int Curtain = Shader.PropertyToID("_Curtain");
        private static readonly int Door = Shader.PropertyToID("_Door");
        private static readonly int Sill = Shader.PropertyToID("_Sill");
        private static readonly int HasWindows = Shader.PropertyToID("_HasWindows");
        private static readonly int WindowOffset = Shader.PropertyToID("_WindowOffset");
        private static readonly int HasDoor = Shader.PropertyToID("_HasDoor");

        public static Material Plain()
        {
            var material = Bricks();
            material.SetFloat(HasWindows, No);
            return material;
        }

        public static Material FrontAndBack()
        {
            return Windowed(FrontWindowsFromTheMiddleMetres, Yes);
        }

        public static Material Gable()
        {
            return Windowed(GableWindowsFromTheMiddleMetres, No);
        }

        private static Material Windowed(float windowsFromTheMiddleMetres, float hasDoor)
        {
            var material = Bricks();
            material.SetFloat(HasWindows, Yes);
            material.SetFloat(WindowOffset, windowsFromTheMiddleMetres);
            material.SetFloat(HasDoor, hasDoor);
            material.SetColor(Frame, BuildingPalette.WindowFrame);
            material.SetColor(Glass, BuildingPalette.Glass);
            material.SetColor(Curtain, BuildingPalette.Curtain);
            material.SetColor(Door, BuildingPalette.FrontDoor);
            material.SetColor(Sill, BuildingPalette.Sill);
            return material;
        }

        private static Material Bricks()
        {
            var material = LookShaders.Made(LookShaders.Brick);
            var brick = BuildingPalette.Brick;
            material.SetColor(LookProperties.Colour, brick.Main);
            material.SetColor(LookProperties.Variation, brick.Worn);
            material.SetColor(LookProperties.Mortar, brick.Accent);
            return material;
        }
    }
}
