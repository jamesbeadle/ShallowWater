using System;
using System.Collections.Generic;
using ShallowWater.Game.Shapes;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class SurfaceLooks
    {
        private static readonly Dictionary<Surface, Func<Material>> BySurface = new Dictionary<Surface, Func<Material>>
        {
            { Surface.Water, WaterLook.Canal },
            { Surface.River, WaterLook.River },
            { Surface.Grass, () => WeatheredLook.Of(CountryPalette.Grass, Weathering.Turf) },
            { Surface.Bank, () => WeatheredLook.Of(CountryPalette.Bank, Weathering.Mud) },
            { Surface.Towpath, () => WeatheredLook.Of(CountryPalette.Towpath, Weathering.Dirt) },
            { Surface.PavedTowpath, () => StoneLook.Of(StonePalette.Paviors, StoneLook.PaviorMetres) },
            { Surface.Coping, () => StoneLook.Of(StonePalette.Coping, StoneLook.CopingMetres) },
            { Surface.Kerb, () => StoneLook.Of(StonePalette.Kerb, StoneLook.KerbMetres) },
            { Surface.Pavement, () => StoneLook.Of(StonePalette.Flagstones, StoneLook.FlagstoneMetres) },
            { Surface.Bollard, () => WeatheredLook.Of(BuildingPalette.Iron, Weathering.CastIron) },
            { Surface.Iron, () => WeatheredLook.Of(BuildingPalette.Iron, Weathering.CastIron) },
            { Surface.Railway, RailwayLook.Track },
            { Surface.MainRoad, () => WeatheredLook.Of(RoadPalette.MainRoad, Weathering.Tar) },
            { Surface.Road, () => WeatheredLook.Of(RoadPalette.Road, Weathering.Tar) },
            { Surface.Lane, () => WeatheredLook.Of(RoadPalette.Lane, Weathering.Gravel) },
            { Surface.RoadLine, () => WeatheredLook.Of(RoadPalette.RoadLine, Weathering.Enamel) },
            { Surface.Hedge, () => HedgeLook.Of(CountryPalette.Hedge, HedgeLook.Haws) },
            { Surface.GardenHedge, () => HedgeLook.Of(CountryPalette.GardenHedge, HedgeLook.NoBerries) },
            { Surface.TelegraphPole, () => WeatheredLook.Of(RoadPalette.TelegraphPole, Weathering.Timber) },
            { Surface.Wall, BrickLook.FrontAndBack },
            { Surface.GableWall, BrickLook.Gable },
            { Surface.Brickwork, BrickLook.Plain },
            { Surface.Roof, () => SlateLook.Of(BuildingPalette.Slate) },
            { Surface.ChimneyPot, () => WeatheredLook.Of(BuildingPalette.ChimneyPot, Weathering.Terracotta) },
            { Surface.Timber, () => WeatheredLook.Of(BuildingPalette.Timber, Weathering.Timber) },
            { Surface.Hull, HullLook.Plates },
            { Surface.RubbingStrake, () => WeatheredLook.Aboard(BoatPalette.Strakes, Weathering.Tarred, BoatSoot.Light) },
            { Surface.Deck, () => WeatheredLook.Aboard(BoatPalette.Deck, Weathering.Timber, BoatSoot.Heavy) },
            { Surface.Cabin, PaintworkLook.CabinSides },
            { Surface.CabinBack, PaintworkLook.CabinDoors },
            { Surface.CabinRoof, () => WeatheredLook.Aboard(BoatPalette.CabinRoof, Weathering.Gloss, BoatSoot.Heavy) },
            { Surface.Cloths, () => WeatheredLook.Aboard(BoatPalette.Cloths, Weathering.Tarpaulin, BoatSoot.Light) },
            { Surface.Cratch, PaintworkLook.Diamonds },
            { Surface.Can, PaintworkLook.Cans },
            { Surface.Brass, () => WeatheredLook.Aboard(BoatPalette.Brass, Weathering.Tarnished, BoatSoot.Light) },
            { Surface.BoatIron, () => WeatheredLook.Aboard(BoatPalette.Iron, Weathering.CastIron, BoatSoot.Light) },
            { Surface.LampGlass, () => WeatheredLook.Aboard(BoatPalette.LampGlass, Weathering.Glass, BoatSoot.Light) },
            { Surface.Rope, RopeLook.Cotton },
            { Surface.TarredRope, RopeLook.TarredHemp },
            { Surface.HorseTail, RopeLook.HorseHair },
            { Surface.OakBark, BarkLook.Oak },
            { Surface.AshBark, BarkLook.Ash },
            { Surface.BirchBark, BarkLook.Birch },
            { Surface.PineBark, BarkLook.Pine },
            { Surface.OakLeaves, FoliageLook.Oak },
            { Surface.AshLeaves, FoliageLook.Ash },
            { Surface.BirchLeaves, FoliageLook.Birch },
            { Surface.PineNeedles, FoliageLook.Pine },
            { Surface.HazelBark, BarkLook.Hazel },
            { Surface.HazelLeaves, FoliageLook.Hazel },
            { Surface.WoodlandFloor, () => WeatheredLook.Of(WoodsPalette.LeafLitter, Weathering.LeafLitter) },
        };

        public static Material Made(Surface surface)
        {
            var isKnown = BySurface.TryGetValue(surface, out var look);
            if (isKnown) return look();
            throw new ArgumentException($"No look is known for the {surface} surface.", nameof(surface));
        }
    }
}
