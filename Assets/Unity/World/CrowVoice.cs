using System.Collections.Generic;
using ShallowWater.Game.Sound;
using ShallowWater.Game.Wildlife;
using ShallowWater.Unity.Sound;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class CrowVoice
    {
        private const string ClipName = "Crow call";
        private const float HeardFromMetres = 4f;
        private const float HeardToMetres = 260f;
        private const float AllAround = 1f;
        private const float PitchSpread = 0.08f;
        private const float NoDoppler = 0f;

        private static readonly Dictionary<QuarryKind, float> PitchOf = new Dictionary<QuarryKind, float>
        {
            { QuarryKind.Crow, 1f }, { QuarryKind.Rook, 0.82f }, { QuarryKind.Magpie, 1.3f }
        };

        private static AudioClip call;

        private readonly AudioSource source;
        private readonly float pitch;

        public CrowVoice(GameObject bird, QuarryKind kind)
        {
            source = bird.AddComponent<AudioSource>();
            source.spatialBlend = AllAround;
            source.dopplerLevel = NoDoppler;
            source.minDistance = HeardFromMetres;
            source.maxDistance = HeardToMetres;
            source.playOnAwake = false;
            pitch = PitchOf[kind];
            call = call ?? Clips.Of(ClipName, CrowCall.Samples(Clips.SamplesPerSecond));
        }

        public void Call()
        {
            source.pitch = pitch + Random.Range(-PitchSpread, PitchSpread);
            source.PlayOneShot(call);
        }
    }
}
