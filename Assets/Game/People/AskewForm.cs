namespace ShallowWater.Game.People
{
    public static class AskewForm
    {
        public const double HipMetres = 0.93;
        public const double HipHalfWidthMetres = 0.1;
        public const double ThighMetres = 0.44;
        public const double ShinMetres = 0.43;
        public const double ShoulderMetres = 1.415;
        public const double ShoulderHalfWidthMetres = 0.18;
        public const double UpperArmMetres = 0.29;
        public const double ForearmMetres = 0.26;
        public const double NeckMetres = 1.49;
        public const double NeckAboveTheHips = NeckMetres - HipMetres;
        public const double BodyDepthShare = 0.78;
        public const double HeadWidthShare = 0.86;
        public const int RoundSides = 18;
        public const int LimbSides = 10;
        public const int FeatureSides = 8;
    }
}
