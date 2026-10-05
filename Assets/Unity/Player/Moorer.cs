using System.Collections.Generic;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Moorer : MonoBehaviour
    {
        private readonly TyingUp job = new TyingUp();
        private readonly MooringGear gear = new MooringGear();
        private BoatController boat;
        private ShoreLeave shore;
        private AskewModel figure;
        private WalkerController walking;
        private Land land;
        private IReadOnlyList<GroundPoint> bollards;
        private MooringPost post;
        private WorldPoint dolly;

        public bool IsLoose => job.IsLoose;
        public bool CanTieUp { get; private set; }
        public bool CanCastOff { get; private set; }

        public void Ready(AskewModel model, WalkerController walker, Land ground, IReadOnlyList<GroundPoint> bollardPlaces)
        {
            boat = GetComponent<BoatController>();
            shore = GetComponent<ShoreLeave>();
            figure = model;
            walking = walker;
            land = ground;
            bollards = bollardPlaces;
        }

        private void Update()
        {
            if (walking == null) return;
            var feet = walking.Position;
            CanTieUp = shore.IsAshore && job.IsLoose && SternLine.CanReach(boat.Motion, feet);
            CanCastOff = shore.IsAshore && job.IsFast && MooringPosts.IsWithinReach(feet, post);
            var isAskedFor = ShoreInput.IsMooring();
            if (isAskedFor && CanTieUp) TieUp(feet);
            if (isAskedFor && CanCastOff) SetToWork();
            if (job.IsBusy) Work();
        }

        private void TieUp(GroundPoint feet)
        {
            post = MooringPosts.For(feet, walking.Bearing, bollards, land);
            dolly = SternLine.DollyNearest(boat.Motion, feet);
            boat.MakeFast();
            SetToWork();
        }

        private void SetToWork()
        {
            job.Begin();
            walking.enabled = false;
            walking.SetOff(MooringPosts.KneelingBeside(walking.Position, post), walking.Bearing);
            walking.Face(post.Place);
            CanTieUp = false;
            CanCastOff = false;
        }

        private void Work()
        {
            job.Work(Time.deltaTime);
            figure.Pose(TyingPose.At(job.Share));
            gear.Show(job.IsLineMade, dolly, post);
            if (job.IsBusy) return;
            walking.enabled = true;
            if (job.IsLoose) boat.CastOff();
        }
    }
}
