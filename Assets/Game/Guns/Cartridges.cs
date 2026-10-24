using System;

namespace ShallowWater.Game.Guns
{
    public sealed class Cartridges
    {
        private const int Empty = 0;
        private const int OneRound = 1;
        private const string Left = " left";

        private Cartridges(string calibre, int boxOf)
        {
            Calibre = calibre;
            BoxOf = boxOf;
            Count = boxOf;
        }

        public string Calibre { get; }
        public int BoxOf { get; }
        public int Count { get; private set; }
        public bool HasAny => Count > Empty;
        public string Words => Count + Left;

        public static Cartridges FullBox(string calibre, int boxOf)
        {
            return new Cartridges(calibre, boxOf);
        }

        public void Spend()
        {
            Count = Math.Max(Empty, Count - OneRound);
        }

        public int Take(int upTo)
        {
            var taken = Math.Min(upTo, Count);
            Count -= taken;
            return taken;
        }
    }
}
