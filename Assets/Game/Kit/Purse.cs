namespace ShallowWater.Game.Kit
{
    public sealed class Purse
    {
        public Purse(int pence)
        {
            Pence = pence;
        }

        public int Pence { get; }
        public string Words => OldMoney.Words(Pence);
    }
}
