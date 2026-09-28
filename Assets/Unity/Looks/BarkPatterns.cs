namespace ShallowWater.Unity.Looks
{
    public static class BarkPatterns
    {
        private const float Furrowed = 0;
        private const float Netted = 1;
        private const float Papery = 2;
        private const float Plated = 3;

        public static readonly BarkPattern OakFurrows = new BarkPattern(Furrowed, 16, 1.1f, 0.025f, 0.35f);
        public static readonly BarkPattern AshNetting = new BarkPattern(Netted, 20, 0.3f, 0.01f, 0.3f);
        public static readonly BarkPattern BirchPaper = new BarkPattern(Papery, 9, 0.3f, 0.004f, 0.08f);
        public static readonly BarkPattern PinePlates = new BarkPattern(Plated, 9, 0.5f, 0.03f, 0.15f);
        public static readonly BarkPattern HazelSmooth = new BarkPattern(Papery, 7, 0.4f, 0.002f, 0.2f);
        public const float BirchFootMetres = 2.2f;
        public const float PineUpperFromMetres = 11f;
    }
}
