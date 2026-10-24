using ShallowWater.Game.Guns;
using ShallowWater.Game.Missions;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Gunfire
    {
        private readonly RifleReport report;
        private readonly Wildlife wildlife;
        private readonly FarmJob job;

        public Gunfire(GameObject firedFrom, Wildlife quarry, FarmJob farmJob)
        {
            report = new RifleReport(firedFrom);
            wildlife = quarry;
            job = farmJob;
        }

        public void Fire(Rifle rifle, Transform sights)
        {
            if (!rifle.IsLoaded) return;
            rifle.Fire();
            report.Fire();
            var muzzle = WorldVectors.PointOf(sights.position);
            var aim = WorldVectors.OffsetOf(sights.forward);
            var struck = Shot.Hit(muzzle, aim, wildlife.Alive, SightLine.ReachOf(sights, rifle.RangeMetres));
            if (struck != null) wildlife.Struck(struck, aim);
            if (struck != null) job.Killed(struck);
            wildlife.Startle(muzzle);
        }
    }
}
