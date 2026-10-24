using ShallowWater.Game.Ground;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.People;
using ShallowWater.Game.Walking;
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
        private WalkerController walking;
        private MooringPosts posts;
        private LineWorker worker;

        public Mooring Mooring => mooring;
        public bool IsHoldingTheLine => mooring.IsLineInHand;
        public bool IsLineOnThePost => mooring.IsCentreLineOnThePost;
        public bool IsWorking => mooring.IsBusy;
        public MooringLine? ToMakeFast { get; private set; }
        public MooringLine? ToLetGo { get; private set; }

        public void Ready(FigureModel model, WalkerController walker, MooringPosts postsAlongTheBank, SurfaceBeneath beneath)
        {
            boat = GetComponent<BoatController>();
            boat.Moored(mooring);
            lines = new MooringLines(GetComponentInChildren<Riding>().transform, beneath);
            holder = new LineHolder(model, walker, boat);
            worker = new LineWorker(model, walker);
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
            walking.HeldBy(mooring.IsLineInHand ? CentreLine.TetherOn(boat.Motion) : Tether.None);
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
            mooring.MakeFast(line, LaidLine.TakenUpBetween(onTheBoat.Tie, post));
            worker.KneelAt(post);
        }

        private void LetGo(MooringLine line)
        {
            mooring.LetGo(line);
            var laid = mooring.LaidAt(line);
            worker.KneelAt(laid.Post);
        }

        private void Work()
        {
            mooring.WorkOn(Time.deltaTime);
            worker.Tie(mooring.Work);
        }
    }
}
