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
            var marksman = boat.AddComponent<Marksman>();
            var talker = boat.AddComponent<Talker>();
            marksman.Ready(job, shore.Figure, shore.Walking, camera, wildlife);
            talker.Ready(job, yard, shore.Walking);
            boat.AddComponent<FarmHud>().Shows(screen, job);
        }
    }
}
