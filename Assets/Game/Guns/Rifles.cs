using System.Collections.Generic;

namespace ShallowWater.Game.Guns
{
    public sealed class Rifles
    {
        public Rifles()
        {
            var rookRifle = new RookRifle();
            var sportingRifle = new SportingRifle();
            All = new Rifle[] { rookRifle, sportingRifle };
            InHand = rookRifle;
            Slung = sportingRifle;
        }

        public IReadOnlyList<Rifle> All { get; }
        public Rifle InHand { get; private set; }
        public Rifle Slung { get; private set; }

        public void Change()
        {
            var taken = Slung;
            Slung = InHand;
            InHand = taken;
        }

        public void Work(double seconds)
        {
            InHand.Work(seconds);
        }
    }
}
