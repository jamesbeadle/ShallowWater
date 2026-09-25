using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundScenery
    {
        private static readonly Color BoatColour = new Color(0.10f, 0.25f, 0.14f);
        private const float SunHeightDegrees = 35f;
        private const float SunBearingDegrees = 140f;
        private const float BoatLengthMetres = 21f;
        private const float BoatBeamMetres = 2.1f;
        private const float BoatHeightMetres = 1.6f;
        private const float CabinLengthMetres = 3f;
        private const float CabinSetBackMetres = 2f;
        private const float CabinShade = 0.7f;

        public static GameObject Boat()
        {
            var hullSize = new Vector3(BoatBeamMetres, BoatHeightMetres, BoatLengthMetres);
            var cabinSize = new Vector3(BoatBeamMetres, BoatHeightMetres, CabinLengthMetres);
            var boat = Slab("Sparrow", BoatColour, hullSize);
            var cabin = Slab("Cabin", BoatColour * CabinShade, cabinSize);
            var cabinPlacement = cabin.transform;
            cabinPlacement.SetParent(boat.transform);
            cabinPlacement.localPosition = new Vector3(0, BoatHeightMetres, -BoatLengthMetres / 2 + CabinSetBackMetres);
            return boat;
        }

        public static void Sun()
        {
            var sun = new GameObject("October sun").AddComponent<Light>();
            sun.type = LightType.Directional;
            var sunPlacement = sun.transform;
            sunPlacement.rotation = Quaternion.Euler(SunHeightDegrees, SunBearingDegrees, 0);
        }

        private static GameObject Slab(string name, Color colour, Vector3 size)
        {
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = name;
            var placement = slab.transform;
            placement.localScale = size;
            var renderer = slab.GetComponent<Renderer>();
            var paint = renderer.material;
            paint.color = colour;
            return slab;
        }
    }
}
