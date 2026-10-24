using ShallowWater.Game.Guns;
using ShallowWater.Game.People;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class ShoulderedRifle
    {
        private readonly FigureModel figure;
        private readonly WalkerController walking;
        private readonly OrbitCamera view;
        private readonly CarriedRifles carried;

        public ShoulderedRifle(FigureModel model, WalkerController walker, OrbitCamera camera, Rifles rifles)
        {
            figure = model;
            walking = walker;
            view = camera;
            carried = new CarriedRifles(model, rifles);
        }

        public Transform Sights => view.transform;

        public void Show(bool isCarried)
        {
            carried.Show(isCarried);
        }

        public void Raise()
        {
            walking.enabled = false;
            carried.Shoulder();
            var rifle = carried.InHand;
            view.Follow(carried.Eye, CameraFraming.Sighting(rifle.Sight));
        }

        public void Lower()
        {
            carried.Carry();
            walking.enabled = true;
            view.Follow(figure.Root, CameraFraming.Ashore);
        }

        public void Change()
        {
            carried.Carry();
        }

        public void Aim()
        {
            walking.SetOff(walking.Position, view.YawDegrees * Mathf.Deg2Rad);
            var muzzleDownDegrees = view.PitchDegrees;
            figure.Pose(AimingPose.At(carried.InHand, muzzleDownDegrees * Mathf.Deg2Rad));
            carried.Point(muzzleDownDegrees);
        }
    }
}
