using ShallowWater.Game.Sound;
using ShallowWater.Unity.Sound;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class RifleReport
    {
        private const string ClipName = "Rifle crack";
        private const float HeardFromMetres = 10f;
        private const float HeardToMetres = 1500f;
        private const float AllAround = 1f;

        private readonly AudioSource source;
        private readonly AudioClip crack;

        public RifleReport(GameObject firedFrom)
        {
            source = firedFrom.AddComponent<AudioSource>();
            source.spatialBlend = AllAround;
            source.minDistance = HeardFromMetres;
            source.maxDistance = HeardToMetres;
            source.playOnAwake = false;
            crack = Clips.Of(ClipName, RifleCrack.Samples(Clips.SamplesPerSecond));
        }

        public void Fire()
        {
            source.PlayOneShot(crack);
        }
    }
}
