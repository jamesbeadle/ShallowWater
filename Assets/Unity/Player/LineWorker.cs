using ShallowWater.Game.Mooring;
using ShallowWater.Game.People;

namespace ShallowWater.Unity.Player
{
    public sealed class LineWorker
    {
        private readonly FigureModel figure;
        private readonly WalkerController walking;
        private MooringPost kneelingAt;

        public LineWorker(FigureModel model, WalkerController walker)
        {
            figure = model;
            walking = walker;
        }

        public void KneelAt(MooringPost post)
        {
            kneelingAt = post;
            walking.enabled = false;
            walking.SetOff(MooringPosts.KneelingBeside(walking.Position, post), walking.Bearing);
            walking.Face(post.Place);
        }

        public void Tie(LineWork work)
        {
            var isFinished = work == null;
            var feet = figure.Root;
            var feetPlace = feet.position;
            var tieAboveTheFeet = kneelingAt.TieMetres - feetPlace.y;
            figure.Pose(TyingPose.At(isFinished ? TyingPose.Finished : work.Share, tieAboveTheFeet));
            if (!isFinished) return;
            walking.enabled = true;
        }
    }
}
