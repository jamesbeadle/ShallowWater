using ShallowWater.Game.Boat;
using ShallowWater.Game.Pound;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundScenery
    {
        private static readonly Color WaterColour = new Color(0.16f, 0.20f, 0.17f);
        private static readonly Color BankColour = new Color(0.30f, 0.34f, 0.18f);
        private static readonly Color BoatColour = new Color(0.10f, 0.25f, 0.14f);
        private const float WaterDepthMetres = 0.1f;
        private const float BankWidthMetres = 40f;
        private const float BankHeightMetres = 0.6f;
        private const float BankTopMetres = 0.5f;
        private const float SunHeightDegrees = 35f;
        private const float SunBearingDegrees = 140f;
        private const float BoatLengthMetres = 21f;
        private const float BoatBeamMetres = 2.1f;
        private const float BoatHeightMetres = 1.6f;
        private const float CabinLengthMetres = 3f;
        private const float CabinSetBackMetres = 2f;
        private const float CabinShade = 0.7f;

        public static void Build(Pound pound)
        {
            var middle = (pound.NorthEndAlong + pound.SouthEndAlong) / 2;
            var length = (float)(pound.SouthEndAlong - pound.NorthEndAlong);
            var bankAcross = pound.HalfWidth + BankWidthMetres / 2;
            var bankCentre = BankTopMetres - BankHeightMetres / 2;
            var alongThePound = Quaternion.Euler(0, (float)pound.BearingAt(middle) * Mathf.Rad2Deg, 0);
            var waterSize = new Vector3((float)pound.HalfWidth * 2, WaterDepthMetres, length);
            var bankSize = new Vector3(BankWidthMetres, BankHeightMetres, length);
            Slab("Water", WaterColour, OnThePound(pound, middle, 0, -WaterDepthMetres / 2), waterSize, alongThePound);
            Slab("East bank", BankColour, OnThePound(pound, middle, -bankAcross, bankCentre), bankSize, alongThePound);
            Slab("West bank", BankColour, OnThePound(pound, middle, bankAcross, bankCentre), bankSize, alongThePound);
            Light();
        }

        public static GameObject Boat()
        {
            var hullSize = new Vector3(BoatBeamMetres, BoatHeightMetres, BoatLengthMetres);
            var cabinSize = new Vector3(BoatBeamMetres, BoatHeightMetres, CabinLengthMetres);
            var boat = Slab("Sparrow", BoatColour, Vector3.zero, hullSize, Quaternion.identity);
            var cabin = Slab("Cabin", BoatColour * CabinShade, Vector3.zero, cabinSize, Quaternion.identity);
            var cabinPlacement = cabin.transform;
            cabinPlacement.SetParent(boat.transform);
            cabinPlacement.localPosition = new Vector3(0, BoatHeightMetres, -BoatLengthMetres / 2 + CabinSetBackMetres);
            return boat;
        }

        private static Vector3 OnThePound(Pound pound, double along, double across, float height)
        {
            var ground = pound.GroundPointAt(new WaterPosition(along, across));
            return new Vector3((float)ground.East, height, (float)ground.North);
        }

        private static GameObject Slab(string name, Color colour, Vector3 position, Vector3 size, Quaternion turn)
        {
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = name;
            var placement = slab.transform;
            placement.position = position;
            placement.rotation = turn;
            placement.localScale = size;
            var renderer = slab.GetComponent<Renderer>();
            var paint = renderer.material;
            paint.color = colour;
            return slab;
        }

        private static void Light()
        {
            var sun = new GameObject("October sun").AddComponent<UnityEngine.Light>();
            sun.type = LightType.Directional;
            var sunPlacement = sun.transform;
            sunPlacement.rotation = Quaternion.Euler(SunHeightDegrees, SunBearingDegrees, 0);
        }
    }
}
