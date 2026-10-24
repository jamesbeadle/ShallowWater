using ShallowWater.Game.Guns;
using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Marksman : MonoBehaviour
    {
        private readonly RookRifle rifle = new RookRifle();
        private ShoreLeave shore;
        private Talker talker;
        private Moorer moorer;
        private FarmJob job;
        private WalkerController walking;
        private Wildlife wildlife;
        private ShoulderedRifle shouldered;
        private RifleReport report;
        private Kit kit;

        public RookRifle Rifle => rifle;
        public bool IsAiming => rifle.IsShouldered;
        public bool CanAim { get; private set; }

        public void Ready(FarmJob farmJob, FigureModel model, WalkerController walker, OrbitCamera camera, Wildlife quarry)
        {
            shore = GetComponent<ShoreLeave>();
            talker = GetComponent<Talker>();
            moorer = GetComponent<Moorer>();
            job = farmJob;
            walking = walker;
            wildlife = quarry;
            shouldered = new ShoulderedRifle(model, walker, camera);
            var root = model.Root;
            report = new RifleReport(root.gameObject);
            var book = GetComponent<PocketBook>();
            kit = book.Kit;
        }

        private void Update()
        {
            if (job == null) return;
            rifle.Work(Time.deltaTime);
            var isCarried = shore.IsAshore;
            shouldered.Show(isCarried);
            CanAim = isCarried && !talker.IsTalking && !moorer.IsWorking;
            var wantsToAim = CanAim && AimInput.IsShouldering();
            if (wantsToAim != IsAiming) Shoulder(wantsToAim);
            if (!IsAiming) return;
            shouldered.Aim();
            if (AimInput.IsFiring()) Fire();
        }

        private void Shoulder(bool isAiming)
        {
            rifle.Shoulder(isAiming);
            if (isAiming) Raise();
            if (!isAiming) Lower();
        }

        private void Raise()
        {
            var hand = moorer.Hand;
            hand.LayDown(walking.Position);
            shouldered.Raise();
        }

        private void Lower()
        {
            shouldered.Lower();
            var hand = moorer.Hand;
            hand.TakeUp();
        }

        private void Fire()
        {
            var cartridges = kit.Cartridges;
            var hasARoundToFire = rifle.IsLoaded && cartridges.HasAny;
            if (!hasARoundToFire) return;
            cartridges.Spend();
            rifle.Fire();
            report.Fire();
            var sights = shouldered.Sights;
            var muzzle = WorldVectors.PointOf(sights.position);
            var aim = WorldVectors.OffsetOf(sights.forward);
            var struck = Shot.Hit(muzzle, aim, wildlife.Alive, SightLine.ReachOf(sights, RookRifle.RangeMetres));
            if (struck != null) wildlife.Struck(struck, aim);
            if (struck != null) job.Killed(struck);
            wildlife.Startle(muzzle);
        }
    }
}
