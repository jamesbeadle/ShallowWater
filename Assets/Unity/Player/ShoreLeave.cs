using ShallowWater.Game.Boat;
using ShallowWater.Game.People;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class ShoreLeave : MonoBehaviour
    {
        private const float StandingToPortMetres = -0.15f;

        private BoatController boat;
        private Pound pound;
        private AskewModel figure;
        private WalkerController walking;
        private OrbitCamera view;
        private Transform deck;
        private Moorer moorer;

        public bool IsAshore { get; private set; }
        public bool CanStepAshore { get; private set; }
        public bool CanStepAboard { get; private set; }
        public bool IsHeldAshoreByTheLine { get; private set; }

        public void Crew(Pound water, Land ground, OrbitCamera camera)
        {
            pound = water;
            view = camera;
            boat = GetComponent<BoatController>();
            deck = GetComponentInChildren<Riding>().transform;
            figure = AskewModel.Made();
            var askew = figure.Root;
            walking = askew.gameObject.AddComponent<WalkerController>();
            walking.Ready(ground, figure, camera);
            moorer = gameObject.AddComponent<Moorer>();
            moorer.Ready(figure, walking, ground, Bollards.AlongThe(water));
            StepAboard();
        }

        private void Update()
        {
            if (boat == null) return;
            var motion = boat.Motion;
            CanStepAshore = !IsAshore && Landing.CanStepAshore(motion, pound);
            var isBesideTheHelm = IsAshore && Landing.CanStepAboard(motion, walking.Position);
            var isLoose = moorer.IsLoose;
            CanStepAboard = isBesideTheHelm && isLoose;
            IsHeldAshoreByTheLine = isBesideTheHelm && !isLoose;
            if (!ShoreInput.IsSteppingAcross()) return;
            if (CanStepAshore) StepAshore();
            if (CanStepAboard) StepAboard();
        }

        private void StepAshore()
        {
            var motion = boat.Motion;
            boat.LeaveTheHelm();
            var askew = figure.Root;
            askew.SetParent(null, true);
            walking.SetOff(Landing.AshoreFrom(motion, pound), motion.Bearing);
            walking.enabled = true;
            view.Follow(askew, CameraFraming.Ashore);
            IsAshore = true;
            CanStepAboard = false;
        }

        private void StepAboard()
        {
            walking.enabled = false;
            var askew = figure.Root;
            askew.SetParent(deck, false);
            var helm = HullOutline.Helm;
            askew.localPosition = new Vector3(StandingToPortMetres, (float)SparrowForm.HatchFloorMetres, (float)helm.Ahead);
            askew.localRotation = Quaternion.identity;
            figure.Pose(Gait.AtTheTiller());
            boat.TakeTheHelm();
            view.Follow(transform, CameraFraming.AtTheHelm);
            IsAshore = false;
        }
    }
}
