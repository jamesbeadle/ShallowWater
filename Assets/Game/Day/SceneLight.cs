namespace ShallowWater.Game.Day
{
    public readonly struct SceneLight
    {
        public SkyPosition Position { get; }
        public bool IsMoonlight { get; }
        public double Strength { get; }

        public SceneLight(SkyPosition position, bool isMoonlight, double strength)
        {
            Position = position;
            IsMoonlight = isMoonlight;
            Strength = strength;
        }
    }
}
