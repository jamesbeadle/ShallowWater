using System;

namespace ShallowWater.Game.Kit
{
    public sealed class Cartridges
    {
        public const string Calibre = ".300 Rook";
        public const int BoxOf = 50;
        private const int Empty = 0;
        private const int OneRound = 1;
        private const string Left = " left";

        private Cartridges(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }
        public bool HasAny => Count > Empty;
        public string Words => Count + Left;

        public static Cartridges FullBox()
        {
            return new Cartridges(BoxOf);
        }

        public void Spend()
        {
            Count = Math.Max(Empty, Count - OneRound);
        }
    }
}
