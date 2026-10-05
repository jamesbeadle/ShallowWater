using ShallowWater.Game.Boat;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class PaintworkLook
    {
        private const float Unused = 0;
        private const float CabinSideDecoration = 1;
        private const float BackDoorsDecoration = 2;
        private const float DiamondsDecoration = 3;
        private const float CanDecoration = 4;
        private const float CabinSoot = 0.45f;
        private const float DoorSoot = 0.55f;
        private const float CratchSoot = 0.5f;
        private const float CanSoot = 0.35f;
        private const float CabinFade = 0.35f;
        private const float DoorFade = 0.5f;
        private const float CratchFade = 0.5f;
        private const float CanFade = 0.4f;
        private static readonly Vector4 NoPanel = new Vector4(Unused, Unused, Unused, Unused);
        private static readonly int FirstPanel = Shader.PropertyToID("_FirstPanel");
        private static readonly int SecondPanel = Shader.PropertyToID("_SecondPanel");
        private static readonly int PanelHeights = Shader.PropertyToID("_PanelHeights");
        private static readonly int Decoration = Shader.PropertyToID("_Decoration");
        private static readonly int Fade = Shader.PropertyToID("_Fade");

        public static Material CabinSides()
        {
            var inset = SparrowForm.PanelInsetMetres;
            var cabin = Span(SparrowForm.CabinBackAlong + inset, SparrowForm.EngineRoomAlong - inset);
            var engineRoom = Span(SparrowForm.EngineRoomAlong + inset, SparrowForm.CabinFrontAlong - inset);
            var panels = Painted(CabinSideDecoration, CabinSoot, CabinFade);
            return Panelled(panels, cabin, engineRoom, Span(SparrowForm.PanelFootMetres, SparrowForm.PanelTopMetres));
        }

        public static Material BackDoors()
        {
            var stile = SparrowForm.DoorStileMetres;
            var hinge = SparrowForm.HatchHalfWidthMetres + stile;
            var free = SparrowForm.HatchHalfWidthMetres + SparrowForm.DoorWidthMetres - stile;
            var doors = Painted(BackDoorsDecoration, DoorSoot, DoorFade);
            var heights = Span(SparrowForm.DoorsFootMetres + stile, SparrowForm.DoorsTopMetres - stile);
            return Panelled(doors, Span(-free, -hinge), Span(hinge, free), heights);
        }

        public static Material Diamonds()
        {
            return Panelled(Painted(DiamondsDecoration, CratchSoot, CratchFade), NoPanel, NoPanel, NoPanel);
        }

        public static Material Cans()
        {
            return Panelled(Painted(CanDecoration, CanSoot, CanFade), NoPanel, NoPanel, NoPanel);
        }

        private static Material Panelled(Material paint, Vector4 firstPanel, Vector4 secondPanel, Vector4 heights)
        {
            paint.SetVector(FirstPanel, firstPanel);
            paint.SetVector(SecondPanel, secondPanel);
            paint.SetVector(PanelHeights, heights);
            return paint;
        }

        private static Material Painted(float decoration, float soot, float fade)
        {
            var paint = LookShaders.Made(LookShaders.Paintwork);
            PaintworkColours.Mix(paint);
            paint.SetFloat(Decoration, decoration);
            paint.SetFloat(LookProperties.Soot, soot);
            paint.SetFloat(Fade, fade);
            return paint;
        }

        private static Vector4 Span(double from, double to)
        {
            return new Vector4((float)from, (float)to, Unused, Unused);
        }
    }
}
