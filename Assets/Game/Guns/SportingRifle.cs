using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Guns
{
    public sealed class SportingRifle : Rifle
    {
        public const string Calibre = ".22 rimfire";
        public const int BoxOf = 100;
        public const int MagazineRounds = 5;
        private const string RifleName = "Sporting rifle";
        private const string RepeaterWords = "Five-shot magazine, telescope sight, " + Calibre;
        private const string OnHisCertificate = "on his certificate";
        private const double Range = 150;
        private const double Balance = 0.5;
        private const double BoltCycleSeconds = 0.6;
        private const double MagazineChangeSeconds = 2.4;
        private const double TelescopePower = 4;
        private const double EyeAheadOfTheButtMetres = 0.17;
        private const double OnTheCentreLine = 0;
        private static readonly Offset Eye = new Offset(OnTheCentreLine, TelescopeShapes.AxisAboveTheButtMetres, EyeAheadOfTheButtMetres);
        private static readonly RifleSight Telescope = RifleSight.Telescope(TelescopePower);

        public SportingRifle() : base(new Magazine(Cartridges.FullBox(Calibre, BoxOf), MagazineRounds, BoltCycleSeconds, MagazineChangeSeconds))
        {
        }

        public override string Name => RifleName;
        public override string Description => RepeaterWords;
        public override string Provenance => OnHisCertificate;
        public override double RangeMetres => Range;
        public override double BalanceFromTheButtMetres => Balance;
        public override Offset EyeFromTheButt => Eye;
        public override RifleSight Sight => Telescope;

        public override SurfaceShapes Shapes()
        {
            return RifleShapes.SportingRifle();
        }
    }
}
