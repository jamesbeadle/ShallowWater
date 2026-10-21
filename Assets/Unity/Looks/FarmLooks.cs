using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class FarmLooks
    {
        private const float Sooted = 0.25f;
        private const float WornIn = 0.15f;
        private const float Clean = 0;

        private static readonly Dictionary<Surface, Func<Material>> BySurface = new Dictionary<Surface, Func<Material>>
        {
            { Surface.Tweed, () => WeatheredLook.Aboard(FarmPalette.Tweed, Weathering.Cloth, WornIn) },
            { Surface.Corduroy, () => WeatheredLook.Aboard(FarmPalette.Corduroy, Weathering.Cloth, Sooted) },
            { Surface.Straw, () => WeatheredLook.Of(FarmPalette.Straw, Weathering.Tarpaulin) },
            { Surface.Thatch, () => WeatheredLook.Of(FarmPalette.Thatch, Weathering.LeafLitter) },
            { Surface.Feathers, () => WeatheredLook.Aboard(FarmPalette.Feathers, Weathering.Gloss, Clean) },
            { Surface.WhiteFeathers, () => WeatheredLook.Aboard(FarmPalette.WhiteFeathers, Weathering.Cloth, Clean) },
            { Surface.Beak, () => WeatheredLook.Aboard(FarmPalette.Beak, Weathering.Leather, Clean) },
            { Surface.Fur, () => WeatheredLook.Aboard(FarmPalette.Fur, Weathering.Cloth, Clean) },
            { Surface.GunStock, () => WeatheredLook.Aboard(FarmPalette.GunStock, Weathering.Timber, Clean) },
            { Surface.GunMetal, () => WeatheredLook.Aboard(FarmPalette.GunMetal, Weathering.Tarnished, Clean) },
        };

        public static Material Made(Surface surface)
        {
            var isKnown = BySurface.TryGetValue(surface, out var look);
            if (isKnown) return look();
            return CropLooks.Made(surface);
        }
    }
}
