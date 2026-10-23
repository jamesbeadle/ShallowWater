using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Walking;

namespace ShallowWater.Game.Mooring
{
    public sealed class MooringPosts
    {
        public const double ReachMetres = 2.5;
        private const double PinAheadMetres = 0.45;
        private const double TieBelowTheBollardTopMetres = 0.15;
        private const double BollardTieMetres = Bollards.TopMetres - TieBelowTheBollardTopMetres;
        private const bool IsABollard = false;
        private const bool IsAPin = true;
        private const double KneelingFromThePostMetres = 0.5;

        private readonly IReadOnlyList<GroundPoint> bollards;
        private readonly Land land;

        public MooringPosts(IReadOnlyList<GroundPoint> bollards, Land land)
        {
            this.bollards = bollards;
            this.land = land;
        }

        public MooringPost For(GroundPoint feet, double bearing)
        {
            var withinReach = bollards.Where(bollard => bollard.DistanceTo(feet) < ReachMetres);
            var nearest = withinReach.OrderBy(bollard => bollard.DistanceTo(feet)).ToList();
            var hasABollardWithinReach = nearest.Any();
            if (!hasABollardWithinReach) return Pinned(feet, bearing);
            var bollard = nearest.First();
            return new MooringPost(bollard, BollardTieMetres, Bollards.RadiusMetres, IsABollard);
        }

        private MooringPost Pinned(GroundPoint feet, double bearing)
        {
            var ahead = feet + GroundPoint.Facing(bearing) * PinAheadMetres;
            var pin = land.IsOpen(ahead) ? ahead : feet;
            var ground = land.HeightAt(pin);
            return new MooringPost(pin, ground + MooringPin.TieAboveTheGroundMetres, MooringPin.RadiusMetres, IsAPin);
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
