using ShallowWater.Game.Guns;
using ShallowWater.Game.Kit;
using ShallowWater.Game.Missions;
using ShallowWater.Game.People;
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
        private FigureModel figure;
        private WalkerController walking;
        private OrbitCamera view;
        private Wildlife wildlife;
        private RifleInHand inHand;
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
            figure = model;
            walking = walker;
            view = camera;
            wildlife = quarry;
            inHand = new RifleInHand(model);
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
            inHand.Show(isCarried);
            CanAim = isCarried && !talker.IsTalking && !moorer.IsHoldingTheLine;
            var wantsToAim = CanAim && AimInput.IsShouldering();
            if (wantsToAim != IsAiming) Shoulder(wantsToAim);
            if (!IsAiming) return;
            Aim();
            if (AimInput.IsFiring()) Fire();
        }

        private void Shoulder(bool isAiming)
        {
            rifle.Shoulder(isAiming);
            walking.enabled = !isAiming && shore.IsAshore;
            view.Follow(figure.Root, isAiming ? CameraFraming.Aiming : CameraFraming.Ashore);
            if (isAiming) inHand.Shoulder();
            if (!isAiming) inHand.Carry();
        }

        private void Aim()
        {
            walking.SetOff(walking.Position, view.YawDegrees * Mathf.Deg2Rad);
            var muzzleDownDegrees = view.PitchDegrees;
            figure.Pose(AimingPose.At(muzzleDownDegrees * Mathf.Deg2Rad));
            inHand.Point(muzzleDownDegrees);
        }

        private void Fire()
        {
            var cartridges = kit.Cartridges;
            var hasARoundToFire = rifle.IsLoaded && cartridges.HasAny;
            if (!hasARoundToFire) return;
            cartridges.Spend();
            rifle.Fire();
            report.Fire();
            var eye = view.transform;
            var muzzle = WorldVectors.PointOf(eye.position);
            var aim = WorldVectors.OffsetOf(eye.forward);
            var struck = Shot.Hit(muzzle, aim, wildlife.Alive, SightLine.ReachOf(eye, RookRifle.RangeMetres));
            if (struck != null) wildlife.Struck(struck, aim);
            if (struck != null) job.Killed(struck);
            wildlife.Startle(muzzle);
        }
    }
}
