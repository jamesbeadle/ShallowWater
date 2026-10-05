using System.Collections.Generic;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class CropSurfaces
    {
        private static readonly Dictionary<Crop, Surface> ByCrop = new Dictionary<Crop, Surface>
        {
            { Crop.Pasture, Surface.Pasture },
            { Crop.RidgeAndFurrow, Surface.RidgeAndFurrow },
            { Crop.WaterMeadow, Surface.WaterMeadow },
            { Crop.CloverLey, Surface.CloverLey },
            { Crop.WheatStubble, Surface.WheatStubble },
            { Crop.BarleyStubble, Surface.BarleyStubble },
            { Crop.OatStubble, Surface.OatStubble },
            { Crop.Ploughland, Surface.Ploughland },
            { Crop.WinterWheat, Surface.WinterWheat },
            { Crop.Mangolds, Surface.Mangolds },
            { Crop.Swedes, Surface.Swedes },
            { Crop.SugarBeet, Surface.SugarBeet },
            { Crop.Potatoes, Surface.Potatoes },
            { Crop.Kale, Surface.Kale },
        };

        public static Surface Of(Crop crop)
        {
            return ByCrop[crop];
        }
    }
}
