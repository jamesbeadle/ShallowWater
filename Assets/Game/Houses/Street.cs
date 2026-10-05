using System;
using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Houses
{
    public sealed class Street
    {
        private const int NoStyles = 0;
        private const double NothingLeftOfTheRoll = 0;
        private static readonly HouseStyle Terrace = new VictorianTerrace();
        private static readonly HouseStyle Cottage = new OldCottage();
        private static readonly HouseStyle Georgian = new GeorgianHouse();

        private static readonly IReadOnlyList<Street> Characters = new List<Street>
        {
            new Street(new Dictionary<HouseStyle, double> { { Terrace, 0.7 }, { Cottage, 0.25 }, { Georgian, 0.05 } }),
            new Street(new Dictionary<HouseStyle, double> { { Terrace, 0.2 }, { Cottage, 0.75 }, { Georgian, 0.05 } }),
            new Street(new Dictionary<HouseStyle, double> { { Terrace, 0.45 }, { Cottage, 0.35 }, { Georgian, 0.2 } }),
        };

        private readonly IReadOnlyDictionary<HouseStyle, double> shares;

        private Street(IReadOnlyDictionary<HouseStyle, double> shares)
        {
            this.shares = shares;
        }

        public static Street Chosen(Random random)
        {
            return Characters[random.Next(Characters.Count)];
        }

        public HouseStyle Pick(Random random, double depthMetres)
        {
            var styles = shares.Keys.Where(style => style.CanStandIn(depthMetres)).ToList();
            var isNothingFitting = styles.Count == NoStyles;
            if (isNothingFitting) return Cottage;
            var roll = random.NextDouble() * styles.Sum(style => shares[style]);
            foreach (var style in styles)
            {
                roll -= shares[style];
                if (roll <= NothingLeftOfTheRoll) return style;
            }
            return styles.Last();
        }
    }
}
