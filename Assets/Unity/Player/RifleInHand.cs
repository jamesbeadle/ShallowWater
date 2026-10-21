using ShallowWater.Game.Guns;
using ShallowWater.Game.People;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class RifleInHand
    {
        private const string Name = "Rook rifle";
        private const float ButtBelowTheShoulderMetres = 0.04f;
        private const float ButtInsideTheShoulderMetres = 0.05f;
        private const float ButtForwardOfTheShoulderMetres = 0.1f;
        private const float AtTheTrailDegrees = 55f;
        private const float Level = 0;
        private static readonly Vector3 ButtAtTheShoulder = new Vector3(
            (float)AskewForm.ShoulderHalfWidthMetres - ButtInsideTheShoulderMetres,
            (float)(AskewForm.ShoulderMetres - AskewForm.HipMetres) - ButtBelowTheShoulderMetres,
            ButtForwardOfTheShoulderMetres);
        private static readonly Vector3 Balance = new Vector3(Level, Level, (float)RookRifle.BalanceFromTheButtMetres);
        private static readonly Vector3 InTheHand = new Vector3(Level, -(float)(AskewForm.ForearmMetres + Hand.PalmBelowTheWristMetres), Level);
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
            Carry();
        }

        public void Show(bool isShown)
        {
            rifle.gameObject.SetActive(isShown);
        }

        public void Carry()
        {
            rifle.SetParent(figure.JointOf(FigurePart.RightForearm), false);
            rifle.localRotation = AtTheTrail;
            rifle.localPosition = InTheHand - AtTheTrail * Balance;
        }

        public void Shoulder()
        {
            rifle.SetParent(figure.JointOf(FigurePart.Body), false);
            Point(Level);
        }

        public void Point(float muzzleDownDegrees)
        {
            rifle.localRotation = Quaternion.Euler(muzzleDownDegrees, Level, Level);
            rifle.localPosition = ButtAtTheShoulder;
        }
    }
}
