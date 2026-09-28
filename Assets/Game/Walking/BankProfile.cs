using System;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;

namespace ShallowWater.Game.Walking
{
    public static class BankProfile
    {
        private const double SlopeMetres = CanalSection.OuterReachMetres - CanalSection.BankTopReachMetres;

        public static double HeightAt(double fromTheCentreMetres)
        {
            var downTheSlope = Math.Clamp((fromTheCentreMetres - CanalSection.BankTopReachMetres) / SlopeMetres, 0, 1);
            return Heights.BankTopMetres + (Heights.GroundMetres - Heights.BankTopMetres) * downTheSlope;
        }
    }
}
