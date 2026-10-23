using ShallowWater.Game.Day;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class PocketWatch
    {
        private const string WatchClass = "sketch-watch";
        private const string CrownClass = "sketch-watch__crown";
        private const string HandClass = "sketch-watch__hand";
        private const string HourClass = "sketch-watch__hand--hour";
        private const string MinuteClass = "sketch-watch__hand--minute";
        private const string PinClass = "sketch-watch__pin";
        private const int HoursOnTheDial = 12;
        private const float DegreesPerHour = 30f;
        private const float DegreesPerMinute = 6f;

        private readonly VisualElement hourHand = OverlayElements.Block(HandClass, HourClass);
        private readonly VisualElement minuteHand = OverlayElements.Block(HandClass, MinuteClass);

        public PocketWatch()
        {
            Face = OverlayElements.Block(WatchClass);
            Face.Add(OverlayElements.Block(CrownClass));
            Face.Add(hourHand);
            Face.Add(minuteHand);
            Face.Add(OverlayElements.Block(PinClass));
        }

        public VisualElement Face { get; }

        public void Show(TimeOfDay time)
        {
            var hoursRoundTheDial = (float)(time.HoursSinceMidnight % HoursOnTheDial);
            Turn(hourHand, hoursRoundTheDial * DegreesPerHour);
            Turn(minuteHand, time.Minute * DegreesPerMinute);
        }

        private static void Turn(VisualElement hand, float degrees)
        {
            var style = hand.style;
            style.rotate = new Rotate(new Angle(degrees));
        }
    }
}
