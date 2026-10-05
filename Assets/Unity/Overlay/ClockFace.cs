using System.Linq;
using ShallowWater.Game.Day;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class ClockFace
    {
        private const char Colon = ':';
        private const string FaceClass = "hud-clock";
        private const string DigitClass = "hud-clock__digit";
        private const string ColonClass = "hud-clock__colon";

        private readonly Label[] figures;
        private string shown = string.Empty;

        public ClockFace(VisualElement screen, TimeOfDay time)
        {
            var reading = time.ToString();
            figures = reading.Select(Figure).ToArray();
            var face = OverlayElements.Glass(FaceClass);
            foreach (var figure in figures) face.Add(figure);
            screen.Add(face);
            Show(time);
        }

        public void Show(TimeOfDay time)
        {
            var reading = time.ToString();
            var isUnchanged = reading == shown;
            if (isUnchanged) return;
            shown = reading;
            for (var place = 0; place < figures.Length; place++) figures[place].text = reading[place].ToString();
        }

        private static Label Figure(char character)
        {
            var isColon = character == Colon;
            return OverlayElements.Heading(character.ToString(), isColon ? ColonClass : DigitClass);
        }
    }
}
