using ShallowWater.Game.Boat;
using ShallowWater.Unity.Helm;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class WakeSignal : MonoBehaviour
    {
        private const float ShortestFrameSeconds = 0.001f;
        private const float Unused = 0;
        private static readonly int BoatPlace = Shader.PropertyToID("_BoatPlace");
        private static readonly int BoatHeading = Shader.PropertyToID("_BoatHeading");
        private static readonly int BoatVelocity = Shader.PropertyToID("_BoatVelocity");
        private static readonly int BoatSizeProperty = Shader.PropertyToID("_BoatSize");

        private BoatController helm;
        private Vector3 lastPlace;

        private void Start()
        {
            helm = GetComponent<BoatController>();
            lastPlace = transform.position;
            var halfBeam = (float)BoatSize.BeamMetres / 2;
            Shader.SetGlobalVector(BoatSizeProperty, new Vector4((float)BoatSize.HalfLengthMetres, halfBeam, Unused, Unused));
        }

        private void LateUpdate()
        {
            var place = transform.position;
            var heading = transform.forward;
            var velocity = (place - lastPlace) / Mathf.Max(Time.deltaTime, ShortestFrameSeconds);
            lastPlace = place;
            var speed = helm == null ? 0 : Mathf.Abs(helm.SpeedShare);
            Shader.SetGlobalVector(BoatPlace, new Vector4(place.x, place.z, Unused, Unused));
            Shader.SetGlobalVector(BoatHeading, new Vector4(heading.x, heading.z, Unused, Unused));
            Shader.SetGlobalVector(BoatVelocity, new Vector4(velocity.x, velocity.y, velocity.z, speed));
        }
    }
}
