using System.Collections.Generic;

namespace ShallowWater.Game.Missions
{
    public static class FarmerLines
    {
        public const string Name = "Pryce";
        public const string Farm = "Dunstall Farm";

        private static readonly Dictionary<FarmJobStage, string[]> ByStage = new Dictionary<FarmJobStage, string[]>
        {
            {
                FarmJobStage.NotYetAsked, new[]
                {
                    "You off the boat? Thought so. Nobody else walks up here.",
                    "That's a rook rifle you've got there. I've rooks in the stackyard and rats under the ricks, and no time to shoot them.",
                    "Clear the lot, every one of them, and I'll put you on to a man who pays for shooting."
                }
            },
            { FarmJobStage.Culling, new[] { "Not yet. I can still hear them." } },
            {
                FarmJobStage.Cleared, new[]
                {
                    "Every one of them. I've not seen shooting like that since France.",
                    "There's a man drinks at the Three Tuns at Fazeley, name of Holland, agent at Tolson's mill. He asked after a man who could shoot.",
                    "Ring Fazeley 14 and say Pryce sent you. Further down the cut, past the junction. You can't miss the mill."
                }
            },
            { FarmJobStage.Paid, new[] { "Fazeley. The Three Tuns. Ask for Holland." } }
        };

        public static IReadOnlyList<string> For(FarmJobStage stage)
        {
            return ByStage[stage];
        }
    }
}
