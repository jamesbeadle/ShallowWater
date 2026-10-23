using System;

namespace ShallowWater.Game.Pound
{
    public static class OldDistance
    {
        private const double YardsPerMetre = 1.0936;
        private const int YardsInAFurlong = 220;
        private const int FurlongsInAMile = 8;
        private const int RoundYardsTo = 10;
        private const int None = 0;
        private const int One = 1;
        private const string Yards = " yards";
        private const string AFurlong = "a furlong";
        private const string Furlongs = " furlongs";
        private const string AMile = "a mile";
        private const string Miles = " miles";
        private const string And = " and ";

        public static string Words(double metres)
        {
            var yards = (int)Math.Round(Math.Abs(metres) * YardsPerMetre / RoundYardsTo) * RoundYardsTo;
            var isUnderAFurlong = yards < YardsInAFurlong;
            if (isUnderAFurlong) return yards + Yards;
            var furlongs = (int)Math.Round((double)yards / YardsInAFurlong);
            var miles = furlongs / FurlongsInAMile;
            var furlongsOver = furlongs % FurlongsInAMile;
            var isUnderAMile = miles == None;
            if (isUnderAMile) return FurlongWords(furlongsOver);
            var isOnTheMile = furlongsOver == None;
            if (isOnTheMile) return MileWords(miles);
            return MileWords(miles) + And + FurlongWords(furlongsOver);
        }

        private static string FurlongWords(int furlongs)
        {
            return furlongs == One ? AFurlong : furlongs + Furlongs;
        }

        private static string MileWords(int miles)
        {
            return miles == One ? AMile : miles + Miles;
        }
    }
}
