using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class WaterLook
    {
        private const float StillWater = 0.6f;
        private const float RunningWater = 1f;
        private const float Half = 0.5f;
        private static readonly int Deep = Shader.PropertyToID("_Deep");
        private static readonly int Shallow = Shader.PropertyToID("_Shallow");
        private static readonly int BankReflection = Shader.PropertyToID("_BankReflection");
        private static readonly int Leaves = Shader.PropertyToID("_Leaves");
        private static readonly int HalfWidth = Shader.PropertyToID("_HalfWidth");
        private static readonly int Ripple = Shader.PropertyToID("_Ripple");

        public static Material Canal()
        {
            var channelHalfWidth = (float)PoundLimits.ChannelHalfWidthMetres;
            return Of(WaterPalette.CanalDeep, WaterPalette.CanalSilt, channelHalfWidth, StillWater);
        }

        public static Material River()
        {
            var riverHalfWidth = (float)LineBands.RiverWidthMetres * Half;
            return Of(WaterPalette.RiverDeep, WaterPalette.RiverSilt, riverHalfWidth, RunningWater);
        }

        private static Material Of(Color deep, Color silt, float halfWidthMetres, float ripple)
        {
            var material = LookShaders.Made(LookShaders.Water);
            material.SetColor(Deep, deep);
            material.SetColor(Shallow, silt);
            material.SetColor(BankReflection, WaterPalette.BanksInTheWater);
            material.SetColor(Leaves, WaterPalette.FallenLeaves);
            material.SetFloat(HalfWidth, halfWidthMetres);
            material.SetFloat(Ripple, ripple);
            return material;
        }
    }
}
