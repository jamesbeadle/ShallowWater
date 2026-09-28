using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class HullLook
    {
        private static readonly int Tar = Shader.PropertyToID("_Tar");
        private static readonly int DustyTar = Shader.PropertyToID("_DustyTar");
        private static readonly int Rust = Shader.PropertyToID("_Rust");
        private static readonly int Weed = Shader.PropertyToID("_Weed");

        public static Material Plates()
        {
            var material = LookShaders.Made(LookShaders.HullPlates);
            material.SetColor(Tar, BoatPalette.Tar);
            material.SetColor(DustyTar, BoatPalette.DustyTar);
            material.SetColor(Rust, BoatPalette.Rust);
            material.SetColor(Weed, BoatPalette.Weed);
            return material;
        }
    }
}
