namespace ShallowWater.Game.People
{
    public readonly struct ArmAngles
    {
        public ArmAngles(double swingRadians, double reachRadians, double elbowRadians)
        {
            SwingRadians = swingRadians;
            ReachRadians = reachRadians;
            ElbowRadians = elbowRadians;
        }

        public double SwingRadians { get; }
        public double ReachRadians { get; }
        public double ElbowRadians { get; }
    }
}
