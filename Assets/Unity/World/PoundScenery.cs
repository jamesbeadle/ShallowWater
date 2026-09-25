using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundScenery
    {
        private static readonly Color WaterColour = new Color(0.16f, 0.20f, 0.17f);
        private static readonly Color BankColour = new Color(0.30f, 0.34f, 0.18f);
        private static readonly Color BoatColour = new Color(0.10f, 0.25f, 0.14f);
        private const float BankWidthMetres = 40f;
        private const float BoatLengthMetres = 21f;
        private const float BoatBeamMetres = 2.1f;
        private const float BoatHeightMetres = 1.6f;

        public static void Build(Pound pound)
        {
            var length = (float)(pound.SouthEndAlong - pound.NorthEndAlong);
            var middle = (float)(pound.NorthEndAlong + pound.SouthEndAlong) / 2f;
            var halfWidth = (float)pound.HalfWidth;
            Slab("Water", WaterColour, new Vector3(0, -0.05f, middle), new Vector3(halfWidth * 2, 0.1f, length));
            Slab("East bank", BankColour, new Vector3(halfWidth + BankWidthMetres / 2, 0.2f, middle), new Vector3(BankWidthMetres, 0.6f, length));
            Slab("West bank", BankColour, new Vector3(-halfWidth - BankWidthMetres / 2, 0.2f, middle), new Vector3(BankWidthMetres, 0.6f, length));
            Light();
        }

        public static GameObject Boat()
        {
            var boat = Slab("Halcyon", BoatColour, Vector3.zero, new Vector3(BoatBeamMetres, BoatHeightMetres, BoatLengthMetres));
            var cabin = Slab("Cabin", BoatColour * 0.7f, Vector3.zero, new Vector3(BoatBeamMetres, BoatHeightMetres, 3f));
            cabin.transform.SetParent(boat.transform);
            cabin.transform.localPosition = new Vector3(0, BoatHeightMetres, -BoatLengthMetres / 2 + 2f);
            return boat;
        }

        private static GameObject Slab(string name, Color colour, Vector3 position, Vector3 size)
        {
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = name;
            slab.transform.position = position;
            slab.transform.localScale = size;
            slab.GetComponent<Renderer>().material.color = colour;
            return slab;
        }

        private static void Light()
        {
            var sun = new GameObject("October sun").AddComponent<UnityEngine.Light>();
            sun.type = LightType.Directional;
            sun.transform.rotation = Quaternion.Euler(35f, 140f, 0);
        }
    }
}
