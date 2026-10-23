using ShallowWater.Game.Boat;
using ShallowWater.Game.Missions;
using ShallowWater.Game.Mooring;
using ShallowWater.Game.Pound;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class DayPage
    {
        private const string Heading = "THE DAY";
        private const string TheMonth = "October 1938";

        private readonly JobEntry job;
        private readonly BoatEntry boat;
        private readonly WaterEntry water;

        public DayPage(VisualElement page)
        {
            page.Add(PageElements.Heading(Heading, TheMonth));
            job = new JobEntry(page);
            boat = new BoatEntry(page);
            water = new WaterEntry(page);
        }

        public void Show(FarmRifleMission mission, ThrottleNotch lever, Mooring mooring, bool isAshore)
        {
            job.Show(mission);
            boat.Show(lever, mooring, isAshore);
        }

        public void Place(Whereabouts whereabouts, string poundLength)
        {
            water.Show(whereabouts, poundLength);
        }
    }
}
