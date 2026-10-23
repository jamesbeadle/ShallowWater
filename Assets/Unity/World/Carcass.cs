using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class Carcass
    {
        private const float MassKilograms = 0.5f;
        private const float RadiusMetres = 0.05f;
        private const float LengthMetres = 0.3f;
        private const float CentreAboveTheFeetMetres = 0.12f;
        private const int AlongTheBody = 2;
        private const float ThrownMetresPerSecond = 1.5f;
        private const float TumbleRadiansPerSecond = 9f;

        public static void Drop(Transform body, Offset aim)
        {
            var fallen = body.gameObject;
            var capsule = fallen.AddComponent<CapsuleCollider>();
            capsule.direction = AlongTheBody;
            capsule.radius = RadiusMetres;
            capsule.height = LengthMetres;
            capsule.center = Vector3.up * CentreAboveTheFeetMetres;
            var weight = fallen.AddComponent<Rigidbody>();
            weight.mass = MassKilograms;
            weight.collisionDetectionMode = CollisionDetectionMode.Continuous;
            weight.interpolation = RigidbodyInterpolation.Interpolate;
            weight.AddForce(WorldVectors.Of(aim.Normalised) * ThrownMetresPerSecond, ForceMode.VelocityChange);
            weight.AddTorque(Random.insideUnitSphere * TumbleRadiansPerSecond, ForceMode.VelocityChange);
        }
    }
}
