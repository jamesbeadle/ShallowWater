using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class JoineryLook
    {
        private static readonly int[] Paints =
        {
            Shader.PropertyToID("_Paint0"), Shader.PropertyToID("_Paint1"), Shader.PropertyToID("_Paint2"), Shader.PropertyToID("_Paint3"),
        };
        private static readonly int[] CurtainColours = { Shader.PropertyToID("_Curtain0"), Shader.PropertyToID("_Curtain1") };
        private static readonly int Glass = Shader.PropertyToID("_Glass");
        private static readonly int Room = Shader.PropertyToID("_Room");
        private static readonly int Net = Shader.PropertyToID("_Net");
        private static readonly int Furniture = Shader.PropertyToID("_Furniture");
        private static readonly int Trim = Shader.PropertyToID("_Trim");

        public static Material Windows()
        {
            var material = Painted(LookShaders.Window, BuildingPalette.WindowPaints);
            material.SetColor(Net, BuildingPalette.NetCurtain);
            for (var curtain = 0; curtain < CurtainColours.Length; curtain++) material.SetColor(CurtainColours[curtain], BuildingPalette.Curtains[curtain]);
            return material;
        }

        public static Material Doors()
        {
            var material = Painted(LookShaders.Door, BuildingPalette.DoorPaints);
            material.SetColor(Furniture, BuildingPalette.DoorFurniture);
            material.SetColor(Trim, BuildingPalette.WindowPaints[0]);
            return material;
        }

        private static Material Painted(string shader, Color[] paints)
        {
            var material = LookShaders.Made(shader);
            for (var paint = 0; paint < Paints.Length; paint++) material.SetColor(Paints[paint], paints[paint]);
            material.SetColor(Glass, BuildingPalette.Glass);
            material.SetColor(Room, BuildingPalette.Room);
            return material;
        }
    }
}
