using ShallowWater.Game.Shapes;
using ShallowWater.Game.Wildlife;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class QuarryBody
    {
        private readonly Transform body;
        private readonly Wings wings;
        private readonly CrowVoice voice;
        private double flyingSeconds;

        private QuarryBody(Quarry quarry, Transform body, double phaseSeconds)
        {
            Quarry = quarry;
            this.body = body;
            flyingSeconds = phaseSeconds;
            var isACorvid = QuarryHabits.IsACorvid(quarry.Kind);
            wings = isACorvid ? Wings.On(body) : null;
            voice = isACorvid ? new CrowVoice(body.gameObject, quarry.Kind) : null;
        }

        public Quarry Quarry { get; }

        public static QuarryBody Of(Quarry quarry, Transform holder, double phaseSeconds)
        {
            var kind = quarry.Kind;
            var body = new GameObject(kind.ToString()).transform;
            body.SetParent(holder, false);
            foreach (var piece in ShapeMeshes.BuildEach(kind.ToString(), QuarryShapes.Of(kind))) Gatherings.Within(body, piece);
            return new QuarryBody(quarry, body, phaseSeconds);
        }

        public void Follow(double seconds)
        {
            if (!Quarry.IsAlive) return;
            var bearingDegrees = (float)Quarry.Bearing * Mathf.Rad2Deg;
            body.SetPositionAndRotation(WorldVectors.Of(Quarry.Position), Quaternion.Euler(0, bearingDegrees, 0));
            if (wings == null) return;
            if (Quarry.IsCalling) voice.Call();
            if (Quarry.IsMoving) Fly(seconds);
            if (!Quarry.IsMoving) wings.Fold();
        }

        public void Fall(Offset aim)
        {
            wings?.Fold();
            Carcass.Drop(body, aim);
        }

        private void Fly(double seconds)
        {
            flyingSeconds += seconds;
            wings.Beat(flyingSeconds);
        }
    }
}
