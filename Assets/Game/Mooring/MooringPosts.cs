using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Mooring
{
    public static class MooringPosts
    {
        public const double ReachMetres = 2.5;
        private const double PinAheadMetres = 0.45;
        private const double TieBelowTheBollardTopMetres = 0.15;
        private const double BollardTieMetres = Bollards.TopMetres - TieBelowTheBollardTopMetres;
        private const bool IsABollard = false;
        private const bool IsAPin = true;
        private const double KneelingFromThePostMetres = 0.5;

        public static MooringPost For(GroundPoint feet, double bearing, IReadOnlyList<GroundPoint> bollards, Land land)
        {
            var withinReach = bollards.Where(bollard => bollard.DistanceTo(feet) < ReachMetres);
            var nearest = withinReach.OrderBy(bollard => bollard.DistanceTo(feet)).ToList();
            var hasABollardWithinReach = nearest.Any();
            if (!hasABollardWithinReach) return Pinned(feet, bearing, land);
            var bollard = nearest.First();
            return new MooringPost(bollard, BollardTieMetres, IsABollard);
        }

        private static MooringPost Pinned(GroundPoint feet, double bearing, Land land)
        {
            var ahead = feet + GroundPoint.Facing(bearing) * PinAheadMetres;
            var pin = land.IsOpen(ahead) ? ahead : feet;
            var ground = land.HeightAt(pin);
            return new MooringPost(pin, ground + MooringPin.TieAboveTheGroundMetres, IsAPin);
        }

        public static GroundPoint KneelingBeside(GroundPoint feet, MooringPost post)
        {
            var away = feet - post.Place;
            var isCloseEnough = away.Length <= KneelingFromThePostMetres;
            if (isCloseEnough) return feet;
            return post.Place + away.Normalised * KneelingFromThePostMetres;
        }

        public static bool IsWithinReach(GroundPoint feet, MooringPost post)
        {
            return feet.DistanceTo(post.Place) < ReachMetres;
        }
    }
}
