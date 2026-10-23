using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Shapes;
using ShallowWater.Game.Wildlife;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class Wildlife : MonoBehaviour
    {
        private const string HolderName = "Wildlife";
        private const double HearingMetres = 160;
        private const double WingbeatsOutOfStepSeconds = 0.07;

        private readonly List<QuarryBody> bodies = new List<QuarryBody>();
        private IReadOnlyList<Flock> flocks = new List<Flock>();

        public IEnumerable<Quarry> Alive => flocks.SelectMany(flock => flock.Alive);

        public static Wildlife Stocked(IReadOnlyList<Flock> flocks)
        {
            var wildlife = new GameObject(HolderName).AddComponent<Wildlife>();
            wildlife.flocks = flocks;
            foreach (var quarry in flocks.SelectMany(flock => flock.All)) wildlife.Embody(quarry);
            return wildlife;
        }

        public void Startle(WorldPoint shotFrom)
        {
            foreach (var flock in flocks) flock.Startle(shotFrom, HearingMetres);
        }

        public void Struck(Quarry quarry, Offset aim)
        {
            quarry.Kill();
            var body = bodies.First(each => each.Quarry == quarry);
            body.Fall(aim);
        }

        private void Embody(Quarry quarry)
        {
            var phaseSeconds = bodies.Count * WingbeatsOutOfStepSeconds;
            bodies.Add(QuarryBody.Of(quarry, transform, phaseSeconds));
        }

        private void Update()
        {
            var seconds = Time.deltaTime;
            foreach (var flock in flocks) flock.Live(seconds);
            foreach (var body in bodies) body.Follow(seconds);
        }
    }
}
