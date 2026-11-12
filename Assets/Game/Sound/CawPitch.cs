namespace ShallowWater.Game.Sound
{
    public static class CawPitch
    {
        private const double OpeningHertz = 560;
        private const double PeakHertz = 660;
        private const double ClosingHertz = 470;
        private const double PeakAtShare = 0.25;

        public static double HertzAt(double share)
        {
            var isRising = share < PeakAtShare;
            if (isRising) return OpeningHertz + (PeakHertz - OpeningHertz) * share / PeakAtShare;
            return PeakHertz + (ClosingHertz - PeakHertz) * (share - PeakAtShare) / (1 - PeakAtShare);
        }
    }
}
