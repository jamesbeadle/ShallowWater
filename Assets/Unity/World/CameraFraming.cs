using ShallowWater.Game.Guns;
using ShallowWater.Unity.Picture;

namespace ShallowWater.Unity.World
{
    public readonly struct CameraFraming
    {
        private const float BehindTheEyeMetres = 0.06f;
        private const float AtTheEye = 0f;
        public static readonly CameraFraming AtTheHelm = new CameraFraming(16f, 2f, 14f, Looking.Round, HelmPicture.FieldOfViewDegrees, isTurnedWithTheTarget: true);
        public static readonly CameraFraming Ashore = new CameraFraming(4.5f, 1.55f, 10f, Looking.Round, HelmPicture.FieldOfViewDegrees);

        public CameraFraming(float distanceMetres, float lookHeightMetres, float pitchDegrees, Looking looking, float fieldOfViewDegrees, bool isTurnedWithTheTarget = false, bool isAtTheEye = false)
        {
            DistanceMetres = distanceMetres;
            LookHeightMetres = lookHeightMetres;
            PitchDegrees = pitchDegrees;
            Looking = looking;
            FieldOfViewDegrees = fieldOfViewDegrees;
            IsTurnedWithTheTarget = isTurnedWithTheTarget;
            IsAtTheEye = isAtTheEye;
        }

        public float DistanceMetres { get; }
        public float LookHeightMetres { get; }
        public float PitchDegrees { get; }
        public Looking Looking { get; }
        public float FieldOfViewDegrees { get; }
        public bool IsTurnedWithTheTarget { get; }
        public bool IsAtTheEye { get; }

        public static CameraFraming Sighting(RifleSight sight)
        {
            var downTheSights = Looking.DownTheSights;
            var magnification = (float)sight.Magnification;
            var fieldOfViewDegrees = (float)sight.FieldOfViewDegrees(HelmPicture.FieldOfViewDegrees);
            return new CameraFraming(BehindTheEyeMetres, AtTheEye, AtTheEye, downTheSights.Through(magnification), fieldOfViewDegrees, isAtTheEye: true);
        }
    }
}
