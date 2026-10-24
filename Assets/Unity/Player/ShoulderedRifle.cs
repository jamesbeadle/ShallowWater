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
        private readonly RifleInHand inHand;

        public ShoulderedRifle(FigureModel model, WalkerController walker, OrbitCamera camera)
        {
            figure = model;
            walking = walker;
            view = camera;
            inHand = new RifleInHand(model);
        }

        public Transform Sights => view.transform;

        public void Show(bool isCarried)
        {
            inHand.Show(isCarried);
        }

        public void Raise()
        {
            walking.enabled = false;
            inHand.Shoulder();
            view.Follow(inHand.Eye, CameraFraming.Sighting);
        }

        public void Lower()
        {
            inHand.Carry();
            walking.enabled = true;
            view.Follow(figure.Root, CameraFraming.Ashore);
        }

        public void Aim()
        {
            walking.SetOff(walking.Position, view.YawDegrees * Mathf.Deg2Rad);
            var muzzleDownDegrees = view.PitchDegrees;
            figure.Pose(AimingPose.At(muzzleDownDegrees * Mathf.Deg2Rad));
            inHand.Point(muzzleDownDegrees);
        }
    }
}
