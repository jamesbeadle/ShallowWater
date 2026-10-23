using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class Sketches
    {
        private const string SketchClass = "sketch";
        private const string RifleClass = "sketch-rifle";
        private const string HookClass = "sketch-hook";
        private const string BoxClass = "sketch-box";
        private const string CoinsClass = "sketch-coins";
        private const string GateClass = "sketch-gate";
        private const string BoatClass = "sketch-boat";
        private const string RopeClass = "sketch-rope";
        private const string FlywheelClass = "sketch-flywheel";
        private const string ReedsClass = "sketch-reeds";
        private const string PartSuffix = "__";

        public static VisualElement Rifle()
        {
            return Drawn(RifleClass, "stock", "wrist", "barrel", "bolt");
        }

        public static VisualElement Hook()
        {
            return Drawn(HookClass, "screw", "curl");
        }

        public static VisualElement CartridgeBox()
        {
            return Drawn(BoxClass, "lid", "label");
        }

        public static VisualElement Coins()
        {
            return Drawn(CoinsClass, "penny", "shilling", "florin");
        }

        public static VisualElement Gate()
        {
            return Drawn(GateClass, "hinge-post", "rail", "rail", "rail", "latch-post");
        }

        public static VisualElement Boat()
        {
            return Drawn(BoatClass, "hull", "cabin", "chimney", "tiller");
        }

        public static VisualElement Rope()
        {
            return Drawn(RopeClass, "coil", "end");
        }

        public static VisualElement Flywheel()
        {
            return Drawn(FlywheelClass, "rim", "spoke", "cross-spoke", "boss");
        }

        public static VisualElement Reeds()
        {
            return Drawn(ReedsClass, "water", "reed", "bent-reed", "far-reed");
        }

        private static VisualElement Drawn(string sketchClass, params string[] parts)
        {
            var sketch = OverlayElements.Block(SketchClass, sketchClass);
            foreach (var part in parts) sketch.Add(OverlayElements.Block(sketchClass + PartSuffix + part));
            return sketch;
        }
    }
}
