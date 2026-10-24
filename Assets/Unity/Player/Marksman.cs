using ShallowWater.Game.Guns;
using ShallowWater.Game.Missions;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Marksman : MonoBehaviour
    {
        private ShoreLeave shore;
        private Talker talker;
        private Moorer moorer;
        private Rifles rifles;
        private WalkerController walking;
        private ShoulderedRifle shouldered;
        private Gunfire gunfire;

        public Rifle Rifle => rifles.InHand;
        public Rifle Slung => rifles.Slung;
        public bool IsAiming => Rifle.IsShouldered;
        public bool CanAim { get; private set; }

        public void Ready(FarmJob farmJob, FigureModel model, WalkerController walker, OrbitCamera camera, Wildlife quarry)
        {
            shore = GetComponent<ShoreLeave>();
            talker = GetComponent<Talker>();
            moorer = GetComponent<Moorer>();
            var book = GetComponent<PocketBook>();
            var kit = book.Kit;
            rifles = kit.Rifles;
            walking = walker;
            shouldered = new ShoulderedRifle(model, walker, camera, rifles);
            var root = model.Root;
            gunfire = new Gunfire(root.gameObject, quarry, farmJob);
        }

        private void Update()
        {
            if (rifles == null) return;
            rifles.Work(Time.deltaTime);
            var isCarried = shore.IsAshore;
            shouldered.Show(isCarried);
            CanAim = isCarried && !talker.IsTalking && !moorer.IsWorking;
            var isChanging = CanAim && AimInput.IsChangingTheRifle();
            if (isChanging) Change();
            var wantsToAim = WantsToAim();
            if (wantsToAim && !IsAiming) Raise();
            if (!wantsToAim && IsAiming) Lower();
            if (!IsAiming) return;
            shouldered.Aim();
            if (AimInput.IsFiring()) gunfire.Fire(Rifle, shouldered.Sights);
        }

        private bool WantsToAim()
        {
            if (!CanAim) return false;
            if (AimInput.IsTogglingTheAim()) return !IsAiming;
            return IsAiming;
        }

        private void Raise()
        {
            Rifle.Shoulder(isShouldered: true);
            var hand = moorer.Hand;
            hand.LayDown(walking.Position);
            shouldered.Raise();
        }

        private void Lower()
        {
            Rifle.Shoulder(isShouldered: false);
            shouldered.Lower();
            var hand = moorer.Hand;
            hand.TakeUp();
        }

        private void Change()
        {
            if (IsAiming) Lower();
            rifles.Change();
            shouldered.Change();
        }
    }
}
