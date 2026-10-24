using ShallowWater.Game.Guns;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class RifleBody
    {
        private const float AtTheTrailDegrees = 55f;
        private const float Level = 0;
        private static readonly Offset AtTheButt = new Offset(Level, Level, Level);
        private static readonly Vector3 InTheHand = new Vector3(Level, -(float)AskewForm.PalmBelowTheWristMetres, Level);
        private static readonly Vector3 ButtByTheRightHip = new Vector3(0.2f, -0.33f, -0.26f);
        private static readonly Vector3 MuzzleOverTheLeftShoulder = new Vector3(-0.34f, 0.61f, -0.19f);
        private static readonly Quaternion AtTheTrail = Quaternion.Euler(AtTheTrailDegrees, Level, Level);
        private static readonly Quaternion AcrossTheBack = Quaternion.LookRotation(MuzzleOverTheLeftShoulder - ButtByTheRightHip, Vector3.back);

        private readonly Rifle rifle;
        private readonly Transform model;
        private readonly Vector3 balance;

        public RifleBody(Rifle rifle)
        {
            this.rifle = rifle;
            model = new GameObject(rifle.Name).transform;
            balance = new Vector3(Level, Level, (float)rifle.BalanceFromTheButtMetres);
            foreach (var piece in ShapeMeshes.BuildEach(rifle.Name, rifle.Shapes()))
            {
                var placement = piece.transform;
                placement.SetParent(model, false);
            }
        }

        public void Show(bool isShown)
        {
            model.gameObject.SetActive(isShown);
        }

        public void Carry(Transform hand)
        {
            model.SetParent(hand, false);
            model.localRotation = AtTheTrail;
            model.localPosition = InTheHand - AtTheTrail * balance;
        }

        public void Sling(Transform back)
        {
            model.SetParent(back, false);
            model.localRotation = AcrossTheBack;
            model.localPosition = ButtByTheRightHip;
        }

        public void Shoulder(Transform root)
        {
            model.SetParent(root, false);
            Point(Level);
        }

        public void Point(float muzzleDownDegrees)
        {
            var butt = AimingPose.OnTheRifle(rifle, AtTheButt, muzzleDownDegrees * Mathf.Deg2Rad);
            model.localRotation = Quaternion.Euler(muzzleDownDegrees, Level, Level);
            model.localPosition = WorldVectors.Of(butt);
        }
    }
}
