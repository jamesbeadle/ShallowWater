using System.Collections.Generic;
using ShallowWater.Game.Missions;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class JobEntry
    {
        private const string Heading = "THE JOB";
        private const string NothingOn = "Nothing on";
        private const string NothingOnNote = "Nobody has asked for anything yet";
        private const string ForTheFarmer = "For " + FarmerLines.Name + ", who farms it";
        private const string WordsClass = "page-job__words";

        private static readonly Dictionary<FarmJobStage, string> Standing = new Dictionary<FarmJobStage, string>
        {
            { FarmJobStage.NotYetAsked, string.Empty },
            { FarmJobStage.Culling, "on it" },
            { FarmJobStage.Cleared, "done, to be paid" },
            { FarmJobStage.Paid, "paid, with a name" }
        };

        private readonly PageEntry job = new PageEntry(Sketches.Gate());
        private readonly Label words = PageElements.Pencil(string.Empty, WordsClass);

        public JobEntry(VisualElement page)
        {
            var section = PageElements.Section(page, Heading);
            section.Add(job.Row);
            section.Add(words);
        }

        public void Show(FarmJob farmJob)
        {
            var hasBegun = farmJob.HasBegun;
            job.Fade(!hasBegun);
            if (!hasBegun) job.Write(NothingOn, NothingOnNote, string.Empty);
            if (hasBegun) job.Write(FarmerLines.Farm, ForTheFarmer, Standing[farmJob.Stage]);
            words.text = farmJob.Words;
        }
    }
}
