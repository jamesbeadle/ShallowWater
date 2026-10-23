using ShallowWater.Game.People;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    [DefaultExecutionOrder(AfterEveryPoser)]
    public sealed class FigureAnimator : MonoBehaviour
    {
        private const int AfterEveryPoser = 1000;
        private const float IdleOffsetAtMostSeconds = 30f;

        private FigureModel figure;
        private float idleOffsetSeconds;

        public void Animate(FigureModel model)
        {
            figure = model;
            idleOffsetSeconds = Random.Range(0f, IdleOffsetAtMostSeconds);
        }

        private void LateUpdate()
        {
            if (figure == null || figure.IsPosedThisFrame) return;
            figure.Pose(Stance.At(Time.time + idleOffsetSeconds));
        }
    }
}
