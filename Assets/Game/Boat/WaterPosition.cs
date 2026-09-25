namespace ShallowWater.Game.Boat
{
    public readonly struct WaterPosition
    {
        public double Along { get; }
        public double Across { get; }

        public WaterPosition(double along, double across)
        {
            Along = along;
            Across = across;
        }

        public WaterPosition Moved(double along, double across)
        {
            return new WaterPosition(Along + along, Across + across);
        }
    }
}
