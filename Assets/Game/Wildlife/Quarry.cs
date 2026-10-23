using System;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Wildlife
{
    public sealed class Quarry
    {
        private readonly Roaming roaming;
        private readonly Calling calling;

        public Quarry(QuarryKind kind, Roaming roaming, Calling calling)
        {
            Kind = kind;
            this.roaming = roaming;
            this.calling = calling;
        }

        public QuarryKind Kind { get; }
        public bool IsAlive { get; private set; } = true;
        public WorldPoint Position => roaming.Position;
        public double Bearing => roaming.Bearing;
        public bool IsMoving => IsAlive && roaming.IsMoving;
        public bool IsCalling => IsAlive && calling.IsCalling;
        public double BodyRadiusMetres => QuarryHabits.Of(Kind).BodyRadiusMetres;

        public void Live(double seconds, Random random)
        {
            if (!IsAlive) return;
            roaming.Work(seconds, random);
            calling.Work(seconds, random);
        }

        public void Startle(Random random)
        {
            if (!IsAlive) return;
            roaming.Startle(random);
            calling.Alarm(random);
        }

        public void Kill()
        {
            IsAlive = false;
        }
    }
}
