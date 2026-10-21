using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Quarry
    {
        private readonly Roaming roaming;

        public Quarry(QuarryKind kind, Roaming roaming)
        {
            Kind = kind;
            this.roaming = roaming;
        }

        public QuarryKind Kind { get; }
        public bool IsAlive { get; private set; } = true;
        public WorldPoint Position => roaming.Position;
        public double Bearing => roaming.Bearing;
        public bool IsMoving => IsAlive && roaming.IsMoving;
        public double BodyRadiusMetres => QuarryHabits.Of(Kind).BodyRadiusMetres;

        public void Live(double seconds, Random random)
        {
            if (IsAlive) roaming.Work(seconds, random);
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}
