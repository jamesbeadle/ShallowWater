using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Fields
{
    public static class Cropping
    {
        private const double NothingLeft = 0;

        private static readonly IReadOnlyList<CropShare> Upland = new[]
        {
            new CropShare(Crop.Pasture, 26), new CropShare(Crop.RidgeAndFurrow, 12), new CropShare(Crop.CloverLey, 8),
            new CropShare(Crop.WheatStubble, 7), new CropShare(Crop.BarleyStubble, 6), new CropShare(Crop.OatStubble, 5),
            new CropShare(Crop.Ploughland, 11), new CropShare(Crop.WinterWheat, 6), new CropShare(Crop.Mangolds, 5),
            new CropShare(Crop.Swedes, 3), new CropShare(Crop.SugarBeet, 5), new CropShare(Crop.Potatoes, 4), new CropShare(Crop.Kale, 2)
        };

        private static readonly IReadOnlyList<CropShare> BesideTheTame = new[]
        {
            new CropShare(Crop.WaterMeadow, 55), new CropShare(Crop.Pasture, 30), new CropShare(Crop.CloverLey, 8), new CropShare(Crop.Ploughland, 7)
        };

        public static Crop For(bool isWaterMeadow, Random random)
        {
            var shares = isWaterMeadow ? BesideTheTame : Upland;
            var roll = random.NextDouble() * shares.Sum(share => share.Weight);
            foreach (var share in shares)
            {
                roll -= share.Weight;
                if (roll < NothingLeft) return share.Crop;
            }
            return shares[0].Crop;
        }

        public static bool IsTilled(Crop crop)
        {
            var isGrass = crop == Crop.Pasture || crop == Crop.RidgeAndFurrow || crop == Crop.WaterMeadow;
            return !isGrass;
        }
    }
}
