using System.Collections.Generic;

namespace ShallowWater.Game.Missions
{
    public static class FarmerLines
    {
        public const string Name = "Pryce";
        public const string Farm = "Dunstall Farm";

        private static readonly Dictionary<FarmRifleStage, string[]> ByStage = new Dictionary<FarmRifleStage, string[]>
        {
            {
                FarmRifleStage.NotYetAsked, new[]
                {
                    "You off the boat? Thought so. Nobody else walks up here.",
                    "I've rooks in the stackyard and rats under the ricks, and a rook rifle I've no time to use.",
                    "Clear the lot, every one of them, and the rifle's yours. Cartridges are in the box by the gate."
                }
            },
            { FarmRifleStage.Culling, new[] { "Not yet. I can still hear them." } },
            {
                FarmRifleStage.Cleared, new[]
                {
                    "Every one of them. I've not seen shooting like that since France.",
                    "Keep the rifle. You'll want it where you're going.",
                    "There's a man drinks at the Three Tuns at Fazeley, name of Holland, agent at Tolson's mill. He asked after a man who could shoot.",
                    "Ring Fazeley 14 and say Pryce sent you. Further down the cut, past the junction. You can't miss the mill."
                }
            },
            { FarmRifleStage.Paid, new[] { "Fazeley. The Three Tuns. Ask for Holland." } }
        };

        public static IReadOnlyList<string> For(FarmRifleStage stage)
        {
            return ByStage[stage];
        }
    }
}
