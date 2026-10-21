using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Missions;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.World
{
    public static class FarmBootstrap
    {
        public static void Begin(GameObject boat, Farmyard yard, FarmyardShapes farm, Land land, OrbitCamera camera, VisualElement screen)
        {
            var shore = boat.GetComponent<ShoreLeave>();
            var wildlife = Wildlife.Stocked(farm, yard);
            var mission = new FarmRifleMission(wildlife.Count);
            FarmerFigure.Standing(yard, land, shore.Askew);
            boat.AddComponent<Marksman>().Ready(mission, shore.Figure, shore.Walking, camera, wildlife);
            boat.AddComponent<Talker>().Ready(mission, yard, shore.Walking);
            boat.AddComponent<FarmHud>().Shows(screen, mission);
        }
    }
}
