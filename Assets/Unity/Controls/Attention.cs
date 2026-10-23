namespace ShallowWater.Unity.Controls
{
    public static class Attention
    {
        public static bool IsReading { get; private set; }

        public static void TurnToTheBook()
        {
            IsReading = true;
        }

        public static void TurnToTheWorld()
        {
            IsReading = false;
        }
    }
}
