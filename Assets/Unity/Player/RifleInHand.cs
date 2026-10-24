using ShallowWater.Game.Guns;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class RifleInHand
    {
        private const string Name = "Rook rifle";
        private const string EyeName = "Aiming eye";
        private const float AtTheTrailDegrees = 55f;
        private const float Level = 0;
        private static readonly Offset AtTheButt = new Offset(Level, Level, Level);
        private static readonly Vector3 Balance = new Vector3(Level, Level, (float)RookRifle.BalanceFromTheButtMetres);
        private static readonly Vector3 InTheHand = new Vector3(Level, -(float)AskewForm.PalmBelowTheWristMetres, Level);
        private static readonly Quaternion AtTheTrail = Quaternion.Euler(AtTheTrailDegrees, Level, Level);

        private readonly FigureModel figure;
        private readonly Transform rifle;

        public RifleInHand(FigureModel figure)
        {
            this.figure = figure;
            rifle = new GameObject(Name).transform;
            foreach (var piece in ShapeMeshes.BuildEach(Name, RifleShapes.Rifle()))
            {
                var placement = piece.transform;
                placement.SetParent(rifle, false);
            }
            Eye = new GameObject(EyeName).transform;
            Eye.SetParent(figure.Root, false);
            Eye.localPosition = WorldVectors.Of(AimingPose.Eye);
            Carry();
        }

        public Transform Eye { get; }

        public void Show(bool isShown)
        {
            rifle.gameObject.SetActive(isShown);
        }

        public void Carry()
        {
            rifle.SetParent(figure.JointOf(FigurePart.RightHand), false);
            rifle.localRotation = AtTheTrail;
            rifle.localPosition = InTheHand - AtTheTrail * Balance;
        }

        public void Shoulder()
        {
            rifle.SetParent(figure.Root, false);
            Point(Level);
        }

        public void Point(float muzzleDownDegrees)
        {
            var butt = AimingPose.OnTheRifle(AtTheButt, muzzleDownDegrees * Mathf.Deg2Rad);
            rifle.localRotation = Quaternion.Euler(muzzleDownDegrees, Level, Level);
            rifle.localPosition = WorldVectors.Of(butt);
        }
    }
}
