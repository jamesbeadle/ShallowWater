using ShallowWater.Game.Ground;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class BrickLook
    {
        private const float Yes = 1;
        private const float No = 0;
        private const float PlinthAboveTheGroundMetres = 0.45f;
        private const float NoPlinthMetres = -100;
        private static readonly int Headers = Shader.PropertyToID("_Headers");
        private static readonly int Plinth = Shader.PropertyToID("_Plinth");
        private static readonly int PlinthTop = Shader.PropertyToID("_PlinthTop");
        private static readonly int Limewash = Shader.PropertyToID("_Limewash");
        private static readonly int IsLimewashed = Shader.PropertyToID("_IsLimewashed");
        private static readonly int IsArch = Shader.PropertyToID("_IsArch");
        private static readonly int Soot = Shader.PropertyToID("_Soot");

        public static Material Walls()
        {
            var material = Bricks(BuildingPalette.BrickFace);
            material.SetFloat(PlinthTop, (float)Heights.GroundMetres + PlinthAboveTheGroundMetres);
            material.SetColor(Plinth, BuildingPalette.BlueBrick);
            return material;
        }

        public static Material Limewashed()
        {
            var material = Walls();
            material.SetFloat(IsLimewashed, Yes);
            material.SetColor(Plinth, BuildingPalette.TarredPlinth);
            return material;
        }

        public static Material Plain()
        {
            return Bricks(BuildingPalette.BrickFace);
        }

        public static Material Arches()
        {
            var material = Bricks(BuildingPalette.RubbedBrick);
            material.SetFloat(IsArch, Yes);
            return material;
        }

        private static Material Bricks(Color face)
        {
            var material = LookShaders.Made(LookShaders.Brick);
            var brick = BuildingPalette.Brick;
            material.SetColor(LookProperties.Colour, face);
            material.SetColor(LookProperties.Variation, brick.Worn);
            material.SetColor(LookProperties.Mortar, brick.Accent);
            material.SetColor(Headers, BuildingPalette.BurntHeader);
            material.SetColor(Limewash, BuildingPalette.Limewash);
            material.SetColor(Soot, BuildingPalette.Soot);
            material.SetFloat(PlinthTop, NoPlinthMetres);
            material.SetFloat(IsLimewashed, No);
            material.SetFloat(IsArch, No);
            return material;
        }
    }
}
