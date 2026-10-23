namespace ShallowWater.Game.People
{
    public readonly struct LimbAngles
    {
        private readonly JointAngles upperSigned;

        public LimbAngles(JointAngles upperSigned, double lowerSwingRadians)
        {
            this.upperSigned = upperSigned;
            LowerSwingRadians = lowerSwingRadians;
        }

        public double LowerSwingRadians { get; }
        public JointAngles Lower => JointAngles.Swung(LowerSwingRadians);

        public JointAngles UpperOf(FigurePart part)
        {
            return JointAxes.Signed(part, upperSigned);
        }
    }
}
