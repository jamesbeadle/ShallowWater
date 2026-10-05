namespace ShallowWater.Unity.Looks
{
    public static class CropPatterns
    {
        private const float NoRows = 1f;
        private const float NoneLifted = 0f;
        private const float Flat = 0f;

        public static readonly CropPattern Pasture = new CropPattern(CropPatternKind.Grass, NoRows, 0.18f, Flat, NoneLifted);
        public static readonly CropPattern RidgeAndFurrow = new CropPattern(CropPatternKind.Grass, NoRows, 0.15f, 0.35f, NoneLifted);
        public static readonly CropPattern WaterMeadow = new CropPattern(CropPatternKind.Grass, NoRows, 0.26f, Flat, NoneLifted);
        public static readonly CropPattern CloverLey = new CropPattern(CropPatternKind.Grass, NoRows, 0.3f, Flat, NoneLifted);
        public static readonly CropPattern HedgeBottom = new CropPattern(CropPatternKind.HedgeBottom, NoRows, 0.6f, Flat, NoneLifted);
        public static readonly CropPattern Footpath = new CropPattern(CropPatternKind.Footpath, NoRows, 0.35f, 0.03f, NoneLifted);
        public static readonly CropPattern WheatStubble = new CropPattern(CropPatternKind.Stubble, 0.18f, 0.12f, 0.02f, NoneLifted);
        public static readonly CropPattern BarleyStubble = new CropPattern(CropPatternKind.Stubble, 0.16f, 0.45f, 0.015f, NoneLifted);
        public static readonly CropPattern OatStubble = new CropPattern(CropPatternKind.Stubble, 0.18f, 0.2f, 0.02f, NoneLifted);
        public static readonly CropPattern Ploughland = new CropPattern(CropPatternKind.Plough, 0.23f, 0.06f, 0.07f, NoneLifted);
        public static readonly CropPattern WinterWheat = new CropPattern(CropPatternKind.Drilled, 0.18f, 0.35f, 0.012f, NoneLifted);
        public static readonly CropPattern Mangolds = new CropPattern(CropPatternKind.Rows, 0.6f, 0.78f, 0.18f, 0.1f);
        public static readonly CropPattern Swedes = new CropPattern(CropPatternKind.Rows, 0.55f, 0.85f, 0.16f, NoneLifted);
        public static readonly CropPattern SugarBeet = new CropPattern(CropPatternKind.Rows, 0.5f, 0.8f, 0.16f, 0.35f);
        public static readonly CropPattern Potatoes = new CropPattern(CropPatternKind.Ridges, 0.7f, 0.5f, 0.12f, 0.45f);
        public static readonly CropPattern Kale = new CropPattern(CropPatternKind.Rows, 0.6f, 0.95f, 0.3f, NoneLifted);
    }
}
