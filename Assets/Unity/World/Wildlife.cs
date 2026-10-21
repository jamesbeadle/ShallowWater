using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Wildlife;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class Wildlife : MonoBehaviour
    {
        private const string HolderName = "Wildlife";
        private const float LaidOnItsSideDegrees = 90f;
        private const double OnTheFloor = 0;

        private readonly Dictionary<Quarry, Transform> bodies = new Dictionary<Quarry, Transform>();
        private Farmyard yard;

        public Flock Flock { get; private set; }
        public IEnumerable<Quarry> Alive => Flock.Alive;
        public int Count => Flock.Count;

        public static Wildlife Stocked(FarmyardShapes farm, Farmyard yard)
        {
            var ratRuns = FarmyardPlan.RatRuns(yard).Select(run => yard.Raised(run, OnTheFloor)).ToList();
            var wildlife = new GameObject(HolderName).AddComponent<Wildlife>();
            wildlife.yard = yard;
            wildlife.Flock = Flock.Stocked(farm.Perches.ToList(), ratRuns);
            var flock = wildlife.Flock;
            foreach (var quarry in flock.All) wildlife.bodies[quarry] = wildlife.Body(quarry);
            return wildlife;
        }

        private void Update()
        {
            Flock.Live(Time.deltaTime);
            foreach (var quarry in Flock.All) Place(quarry, bodies[quarry]);
        }

        private Transform Body(Quarry quarry)
        {
            var body = new GameObject(quarry.Kind.ToString()).transform;
            body.SetParent(transform, false);
            foreach (var piece in ShapeMeshes.BuildEach(quarry.Kind.ToString(), QuarryShapes.Of(quarry.Kind)))
            {
                var placement = piece.transform;
                placement.SetParent(body, false);
            }
            return body;
        }

        private void Place(Quarry quarry, Transform body)
        {
            var position = WorldVectors.Of(quarry.Position);
            var bearing = (float)quarry.Bearing * Mathf.Rad2Deg;
            var standing = Quaternion.Euler(0, bearing, 0);
            if (quarry.IsAlive) body.SetPositionAndRotation(position, standing);
            if (quarry.IsAlive) return;
            position.y = (float)yard.FloorMetres;
            body.SetPositionAndRotation(position, Quaternion.Euler(0, bearing, LaidOnItsSideDegrees));
        }
    }
}
