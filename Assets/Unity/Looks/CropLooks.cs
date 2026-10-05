using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class CropLooks
    {
        private static readonly Dictionary<Surface, Func<Material>> BySurface = new Dictionary<Surface, Func<Material>>
        {
            { Surface.FieldMargin, () => CropLook.Of(CropPalette.HedgeBottom, CropPatterns.HedgeBottom) },
            { Surface.Footpath, () => CropLook.Of(CropPalette.Footpath, CropPatterns.Footpath) },
            { Surface.Pasture, () => CropLook.Of(CropPalette.Pasture, CropPatterns.Pasture) },
            { Surface.RidgeAndFurrow, () => CropLook.Of(CropPalette.OldTurf, CropPatterns.RidgeAndFurrow) },
            { Surface.WaterMeadow, () => CropLook.Of(CropPalette.WaterMeadow, CropPatterns.WaterMeadow) },
            { Surface.CloverLey, () => CropLook.Of(CropPalette.CloverLey, CropPatterns.CloverLey) },
            { Surface.WheatStubble, () => CropLook.Of(CropPalette.WheatStubble, CropPatterns.WheatStubble) },
            { Surface.BarleyStubble, () => CropLook.Of(CropPalette.BarleyStubble, CropPatterns.BarleyStubble) },
            { Surface.OatStubble, () => CropLook.Of(CropPalette.OatStubble, CropPatterns.OatStubble) },
            { Surface.Ploughland, () => CropLook.Of(CropPalette.Ploughland, CropPatterns.Ploughland) },
            { Surface.WinterWheat, () => CropLook.Of(CropPalette.WinterWheat, CropPatterns.WinterWheat) },
            { Surface.Mangolds, () => CropLook.Of(CropPalette.Mangolds, CropPatterns.Mangolds) },
            { Surface.Swedes, () => CropLook.Of(CropPalette.Swedes, CropPatterns.Swedes) },
            { Surface.SugarBeet, () => CropLook.Of(CropPalette.SugarBeet, CropPatterns.SugarBeet) },
            { Surface.Potatoes, () => CropLook.Of(CropPalette.Potatoes, CropPatterns.Potatoes) },
            { Surface.Kale, () => CropLook.Of(CropPalette.Kale, CropPatterns.Kale) },
        };

        public static Material Made(Surface surface)
        {
            var isKnown = BySurface.TryGetValue(surface, out var look);
            if (isKnown) return look();
            throw new ArgumentException($"No look is known for the {surface} surface.", nameof(surface));
        }
    }
}
