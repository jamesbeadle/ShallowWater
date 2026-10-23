namespace ShallowWater.Game.People
{
    public static class AskewForm
    {
        public const double HipMetres = 0.93;
        public const double HipHalfWidthMetres = 0.1;
        public const double WaistAboveTheHipsMetres = 0.18;
        public const double ShoulderAboveTheWaistMetres = 0.31;
        public const double ShoulderHalfWidthMetres = 0.195;
        public const double NeckAboveTheWaistMetres = 0.39;
        public const double ThighMetres = 0.44;
        public const double ShinMetres = 0.43;
        public const double AnkleAboveTheSoleMetres = 0.06;
        public const double KneeRadiusMetres = 0.072;
        public const double UpperArmMetres = 0.3;
        public const double ForearmMetres = 0.26;
        public const double PalmBelowTheWristMetres = 0.07;
        public const double HeelBehindTheAnkleMetres = 0.07;
        public const double ToeAheadOfTheAnkleMetres = 0.19;
        public const double WaistMetres = HipMetres + WaistAboveTheHipsMetres;
        public const double ShoulderMetres = WaistMetres + ShoulderAboveTheWaistMetres;
        public const double NeckMetres = WaistMetres + NeckAboveTheWaistMetres;
        public const double LegMetres = ThighMetres + ShinMetres;
        public const double HandReachMetres = ForearmMetres + PalmBelowTheWristMetres;
        public const double BodyDepthShare = 0.78;
        public const double HeadWidthShare = 0.86;
        public const int RoundSides = 18;
        public const int LimbSides = 10;
        public const int FeatureSides = 8;
    }
}
