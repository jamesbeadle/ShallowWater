using ShallowWater.Game.Ground;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.People;
using ShallowWater.Unity.Helm;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Moorer : MonoBehaviour
    {
        private readonly Mooring mooring = new Mooring();
        private MooringLines lines;
        private LineHolder holder;
        private BoatController boat;
        private FigureModel figure;
        private WalkerController walking;
        private MooringPosts posts;

        public bool IsHoldingTheLine => mooring.IsLineInHand;
        public bool IsLineOnThePost => mooring.IsCentreLineOnThePost;
        public bool IsWorking => mooring.IsBusy;
        public MooringLine? ToMakeFast { get; private set; }
        public MooringLine? ToLetGo { get; private set; }

        public void Ready(FigureModel model, WalkerController walker, MooringPosts postsAlongTheBank)
        {
            boat = GetComponent<BoatController>();
            boat.Moored(mooring);
            lines = new MooringLines(GetComponentInChildren<Riding>().transform);
            holder = new LineHolder(model, walker, boat);
            figure = model;
            walking = walker;
            posts = postsAlongTheBank;
        }

        public void TakeTheLineAshore()
        {
            mooring.TakeAshore(walking.Position);
        }

        public void TakeTheLineAboard()
        {
            mooring.TakeAboard();
        }

        private void Update()
        {
            if (walking == null) return;
            var feet = walking.Position;
            ToMakeFast = MooringChoices.ToMakeFast(mooring, boat.Motion, feet);
            ToLetGo = MooringChoices.ToLetGo(mooring, feet);
            var isMakingFast = ToMakeFast.HasValue && ShoreInput.IsMakingFast();
            if (isMakingFast) MakeFast(ToMakeFast.Value, feet);
            var isLettingGo = ToLetGo.HasValue && ShoreInput.IsLettingGo();
            if (isLettingGo) LetGo(ToLetGo.Value);
            if (mooring.IsBusy) Work();
        }

        private void LateUpdate()
        {
            if (walking == null) return;
            var isWalkingHerAlong = mooring.IsLineInHand && !mooring.IsBusy;
            if (isWalkingHerAlong) holder.Hold(mooring);
            lines.Show(mooring, boat.Motion, holder.HeldEnd);
        }

        private void MakeFast(MooringLine line, GroundPoint feet)
        {
            var post = posts.For(feet, walking.Bearing);
            var onTheBoat = LineEnds.OnTheBoat(line, boat.Motion, feet);
            mooring.MakeFast(line, LaidLine.TakenUpBetween(onTheBoat, post));
            KneelAt(post);
        }

        private void LetGo(MooringLine line)
        {
            mooring.LetGo(line);
            KneelAt(mooring.LaidAt(line).Post);
        }

        private void KneelAt(MooringPost post)
        {
            walking.enabled = false;
            walking.SetOff(MooringPosts.KneelingBeside(walking.Position, post), walking.Bearing);
            walking.Face(post.Place);
        }

        private void Work()
        {
            mooring.WorkOn(Time.deltaTime);
            var work = mooring.Work;
            var isFinished = work == null;
            figure.Pose(TyingPose.At(isFinished ? TyingPose.Finished : work.Share));
            if (!isFinished) return;
            walking.enabled = true;
        }
    }
}
