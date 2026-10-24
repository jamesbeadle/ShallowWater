using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Guns
{
    public sealed class RookRifle : Rifle
    {
        public const string Calibre = ".300 Rook";
        public const int BoxOf = 50;
        private const string RifleName = "Rook rifle";
        private const string SingleShotWords = "Single shot, " + Calibre;
        private const string HisOwn = "his own";
        private const double Range = 90;
        private const double Balance = 0.45;
        private const double ReloadSeconds = 1.6;
        private const double EyeAheadOfTheButtMetres = 0.16;
        private const double OnTheCentreLine = 0;
        private static readonly Offset Eye = new Offset(OnTheCentreLine, RifleShapes.SightsAboveTheButtMetres, EyeAheadOfTheButtMetres);

        public RookRifle() : base(new SingleShot(Cartridges.FullBox(Calibre, BoxOf), ReloadSeconds))
        {
        }

        public override string Name => RifleName;
        public override string Description => SingleShotWords;
        public override string Provenance => HisOwn;
        public override double RangeMetres => Range;
        public override double BalanceFromTheButtMetres => Balance;
        public override Offset EyeFromTheButt => Eye;
        public override RifleSight Sight => RifleSight.Open;

        public override SurfaceShapes Shapes()
        {
            return RifleShapes.RookRifle();
        }
    }
}
