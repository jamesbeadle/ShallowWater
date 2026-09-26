using ShallowWater.Game.Boat;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class PoundScenery
    {
        private static readonly Color BoatColour = new Color(0.10f, 0.25f, 0.14f);
        private const string SunName = "October sun";
        private const float SunHeightDegrees = 35f;
        private const float SunBearingDegrees = 140f;
        private const float BoatLengthMetres = (float)BoatSize.LengthMetres;
        private const float BoatBeamMetres = (float)BoatSize.BeamMetres;
        private const float BoatHeightMetres = 1.6f;
        private const float CabinLengthMetres = 3f;
        private const float CabinSetBackMetres = 2f;
        private const float CabinShade = 0.7f;

        public static GameObject Boat()
        {
            var hullSize = new Vector3(BoatBeamMetres, BoatHeightMetres, BoatLengthMetres);
            var cabinSize = new Vector3(BoatBeamMetres, BoatHeightMetres, CabinLengthMetres);
            var cabinPosition = new Vector3(0, BoatHeightMetres, -BoatLengthMetres / 2 + CabinSetBackMetres);
            var boat = Slab("Sparrow", BoatColour, new Placement(Vector3.zero, hullSize, Quaternion.identity));
            var cabin = Slab("Cabin", BoatColour * CabinShade, new Placement(cabinPosition, cabinSize, Quaternion.identity));
            var cabinPlacement = cabin.transform;
            cabinPlacement.SetParent(boat.transform);
            return boat;
        }

        public static void Sun()
        {
            var sun = SunOfTheScene();
            sun.type = LightType.Directional;
            var sunPlacement = sun.transform;
            sunPlacement.rotation = Quaternion.Euler(SunHeightDegrees, SunBearingDegrees, 0);
        }

        private static Light SunOfTheScene()
        {
            var sceneLight = Object.FindFirstObjectByType<Light>();
            var hasALight = sceneLight != null;
            if (hasALight) return sceneLight;
            var sun = new GameObject(SunName);
            return sun.AddComponent<Light>();
        }

        private static GameObject Slab(string name, Color colour, Placement placement)
        {
            return Blocks.Place(PrimitiveType.Cube, name, Paint.Of(colour), placement);
        }
    }
}
