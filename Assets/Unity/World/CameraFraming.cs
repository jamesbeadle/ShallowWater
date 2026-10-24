namespace ShallowWater.Unity.World
{
    public readonly struct CameraFraming
    {
        public static readonly CameraFraming AtTheHelm = new CameraFraming(16f, 2f, 14f, Looking.Round, isTurnedWithTheTarget: true);
        public static readonly CameraFraming Ashore = new CameraFraming(4.5f, 1.55f, 10f, Looking.Round);
        public static readonly CameraFraming Sighting = new CameraFraming(0.06f, 0f, 0f, Looking.DownTheSights, isAtTheEye: true);

        public CameraFraming(float distanceMetres, float lookHeightMetres, float pitchDegrees, Looking looking, bool isTurnedWithTheTarget = false, bool isAtTheEye = false)
        {
            DistanceMetres = distanceMetres;
            LookHeightMetres = lookHeightMetres;
            PitchDegrees = pitchDegrees;
            Looking = looking;
            IsTurnedWithTheTarget = isTurnedWithTheTarget;
            IsAtTheEye = isAtTheEye;
        }

        public float DistanceMetres { get; }
        public float LookHeightMetres { get; }
        public float PitchDegrees { get; }
        public Looking Looking { get; }
        public bool IsTurnedWithTheTarget { get; }
        public bool IsAtTheEye { get; }
    }
}
