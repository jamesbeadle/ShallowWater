namespace ShallowWater.Game.Woods
{
    public sealed class LimbDetail
    {
        private const int TrunkOrder = 0;
        private readonly int[] sidesByOrder;
        private readonly double thinnestMetres;
        private readonly int everyNthRing;

        public LimbDetail(int[] sidesByOrder, double thinnestMetres, int everyNthRing)
        {
            this.sidesByOrder = sidesByOrder;
            this.thinnestMetres = thinnestMetres;
            this.everyNthRing = everyNthRing;
        }

        public bool IsShown(Limb limb)
        {
            var isTheTrunk = limb.Order == TrunkOrder;
            var isDrawnAtThisDetail = limb.Order < sidesByOrder.Length;
            return isTheTrunk || (isDrawnAtThisDetail && limb.RadiiMetres[0] > thinnestMetres);
        }

        public Limb Coarsened(Limb limb)
        {
            return limb.Coarsened(everyNthRing);
        }

        public int SidesOf(Limb limb)
        {
            return sidesByOrder[limb.Order];
        }
    }
}
