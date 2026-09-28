using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class PaintworkColours
    {
        private static readonly int Frame = Shader.PropertyToID("_Frame");
        private static readonly int Panel = Shader.PropertyToID("_Panel");
        private static readonly int Line = Shader.PropertyToID("_Line");
        private static readonly int Lettering = Shader.PropertyToID("_Lettering");
        private static readonly int LetterShade = Shader.PropertyToID("_LetterShade");
        private static readonly int Primer = Shader.PropertyToID("_Primer");

        public static void Mix(Material paint)
        {
            paint.SetColor(Frame, BoatPalette.CabinFrame);
            paint.SetColor(Panel, BoatPalette.CabinPanel);
            paint.SetColor(Line, BoatPalette.CoachLine);
            paint.SetColor(Lettering, BoatPalette.Lettering);
            paint.SetColor(LetterShade, BoatPalette.LetterShade);
            paint.SetColor(Primer, BoatPalette.Primer);
            paint.SetColor(LookProperties.SootColour, BoatPalette.Soot);
        }
    }
}
