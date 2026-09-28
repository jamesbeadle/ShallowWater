namespace ShallowWater.Unity.World
{
    public readonly struct CameraFraming
    {
        public static readonly CameraFraming AtTheHelm = new CameraFraming(16f, 2f, 14f, true);

        public CameraFraming(float distanceMetres, float lookHeightMetres, float pitchDegrees, bool isTurnedWithTheTarget)
        {
            DistanceMetres = distanceMetres;
            LookHeightMetres = lookHeightMetres;
            PitchDegrees = pitchDegrees;
            IsTurnedWithTheTarget = isTurnedWithTheTarget;
        }

        public float DistanceMetres { get; }
        public float LookHeightMetres { get; }
        public float PitchDegrees { get; }
        public bool IsTurnedWithTheTarget { get; }
    }
}
