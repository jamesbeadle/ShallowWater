using ShallowWater.Game.Boat;
using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class LiveryLook
    {
        private const float Unused = 0;
        private static readonly int Frame = Shader.PropertyToID("_Frame");
        private static readonly int Panel = Shader.PropertyToID("_Panel");
        private static readonly int Line = Shader.PropertyToID("_Line");
        private static readonly int FirstPanel = Shader.PropertyToID("_FirstPanel");
        private static readonly int SecondPanel = Shader.PropertyToID("_SecondPanel");
        private static readonly int PanelHeights = Shader.PropertyToID("_PanelHeights");

        public static Material CabinSides()
        {
            var inset = SparrowForm.PanelInsetMetres;
            var cabin = Span(SparrowForm.CabinBackAlong + inset, SparrowForm.EngineRoomAlong - inset);
            var engineRoom = Span(SparrowForm.EngineRoomAlong + inset, SparrowForm.CabinFrontAlong - inset);
            return Painted(cabin, engineRoom, Span(SparrowForm.PanelFootMetres, SparrowForm.PanelTopMetres));
        }

        public static Material CabinDoors()
        {
            var edge = SparrowForm.CabinHalfWidthMetres - SparrowForm.PanelInsetMetres;
            var portDoor = Span(-edge, -SparrowForm.DoorsMeetMetres);
            var starboardDoor = Span(SparrowForm.DoorsMeetMetres, edge);
            return Painted(portDoor, starboardDoor, Span(SparrowForm.DoorsFootMetres, SparrowForm.PanelTopMetres));
        }

        private static Material Painted(Vector4 firstPanel, Vector4 secondPanel, Vector4 heights)
        {
            var material = LookShaders.Made(LookShaders.Livery);
            material.SetColor(Frame, BoatPalette.CabinFrame);
            material.SetColor(Panel, BoatPalette.CabinPanel);
            material.SetColor(Line, BoatPalette.CoachLine);
            material.SetVector(FirstPanel, firstPanel);
            material.SetVector(SecondPanel, secondPanel);
            material.SetVector(PanelHeights, heights);
            return material;
        }

        private static Vector4 Span(double from, double to)
        {
            return new Vector4((float)from, (float)to, Unused, Unused);
        }
    }
}
