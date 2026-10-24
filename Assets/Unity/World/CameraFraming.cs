namespace ShallowWater.Unity.World
{
    public readonly struct CameraFraming
    {
        public static readonly CameraFraming AtTheHelm = new CameraFraming(16f, 2f, 14f, true, Looking.Round);
        public static readonly CameraFraming Ashore = new CameraFraming(4.5f, 1.55f, 10f, false, Looking.Round);
        public static readonly CameraFraming Sighting = new CameraFraming(0.06f, 0f, 0f, false, Looking.DownTheSights);

        public CameraFraming(float distanceMetres, float lookHeightMetres, float pitchDegrees, bool isTurnedWithTheTarget, Looking looking)
        {
            DistanceMetres = distanceMetres;
            LookHeightMetres = lookHeightMetres;
            PitchDegrees = pitchDegrees;
            IsTurnedWithTheTarget = isTurnedWithTheTarget;
            Looking = looking;
        }

        public float DistanceMetres { get; }
        public float LookHeightMetres { get; }
        public float PitchDegrees { get; }
        public bool IsTurnedWithTheTarget { get; }
        public Looking Looking { get; }
    }
}
