using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Missions;
using ShallowWater.Game.Paddock;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.World
{
    public static class FarmBootstrap
    {
        public static void Begin(GameObject boat, Scenery scenery, Land land, OrbitCamera camera, VisualElement screen)
        {
            var shore = boat.GetComponent<ShoreLeave>();
            var farmFlock = FarmyardStock.Stocked(scenery.Farm);
            var paddockFlock = PaddockStock.Stocked(scenery.Paddock);
            var wildlife = Wildlife.Stocked(new[] { farmFlock, paddockFlock });
            var job = new FarmJob(farmFlock);
            var yard = scenery.Yard;
            FarmerFigure.Standing(yard, land, shore.Askew);
            boat.AddComponent<Marksman>().Ready(job, shore.Figure, shore.Walking, camera, wildlife);
            boat.AddComponent<Talker>().Ready(job, yard, shore.Walking);
            boat.AddComponent<FarmHud>().Shows(screen, job);
        }
    }
}
