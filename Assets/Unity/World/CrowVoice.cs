using System.Collections.Generic;
using System.Linq;
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
        private const float PitchSpread = 0.06f;
        private const float NoDoppler = 0f;
        private const float Voice = 0.6f;
        private const float QuietestCall = 0.7f;
        private const float LoudestCall = 1f;

        private static readonly Dictionary<QuarryKind, float> PitchOf = new Dictionary<QuarryKind, float>
        {
            { QuarryKind.Crow, 1f }, { QuarryKind.Rook, 0.86f }, { QuarryKind.Magpie, 1.25f }
        };

        private static IReadOnlyList<AudioClip> calls;

        private readonly AudioSource source;
        private readonly float pitch;

        public CrowVoice(GameObject bird, QuarryKind kind)
        {
            source = bird.AddComponent<AudioSource>();
            source.spatialBlend = AllAround;
            source.dopplerLevel = NoDoppler;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = HeardFromMetres;
            source.maxDistance = HeardToMetres;
            source.volume = Voice;
            source.playOnAwake = false;
            pitch = PitchOf[kind];
            calls = calls ?? CrowCall.Bouts(Clips.SamplesPerSecond).Select(bout => Clips.Of(ClipName, bout)).ToList();
        }

        public void Call()
        {
            var call = calls[Random.Range(0, calls.Count)];
            source.pitch = pitch + Random.Range(-PitchSpread, PitchSpread);
            source.PlayOneShot(call, Random.Range(QuietestCall, LoudestCall));
        }
    }
}
