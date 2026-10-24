using System.Collections.Generic;
using ShallowWater.Game.Guns;
using ShallowWater.Game.People;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class CarriedRifles
    {
        private const string EyeName = "Aiming eye";

        private readonly FigureModel figure;
        private readonly Rifles rifles;
        private readonly Dictionary<Rifle, RifleBody> models = new Dictionary<Rifle, RifleBody>();

        public CarriedRifles(FigureModel figure, Rifles rifles)
        {
            this.figure = figure;
            this.rifles = rifles;
            foreach (var rifle in rifles.All) models[rifle] = new RifleBody(rifle);
            Eye = new GameObject(EyeName).transform;
            Eye.SetParent(figure.Root, false);
            Eye.localPosition = WorldVectors.Of(AimingPose.Eye);
            Carry();
        }

        public Transform Eye { get; }
        public Rifle InHand => rifles.InHand;

        public void Show(bool isShown)
        {
            foreach (var model in models.Values) model.Show(isShown);
        }

        public void Carry()
        {
            var inHand = models[rifles.InHand];
            inHand.Carry(figure.JointOf(FigurePart.RightHand));
            var slung = models[rifles.Slung];
            slung.Sling(figure.JointOf(FigurePart.Chest));
        }

        public void Shoulder()
        {
            var inHand = models[rifles.InHand];
            inHand.Shoulder(figure.Root);
        }

        public void Point(float muzzleDownDegrees)
        {
            var inHand = models[rifles.InHand];
            inHand.Point(muzzleDownDegrees);
        }
    }
}
