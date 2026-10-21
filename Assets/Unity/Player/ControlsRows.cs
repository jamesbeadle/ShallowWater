using ShallowWater.Unity.Overlay;

namespace ShallowWater.Unity.Player
{
    public static class ControlsRows
    {
        public static readonly HudPrompt[] AtTheHelm =
        {
            new HudPrompt("W|S", "Lever ahead and astern"), new HudPrompt("A|D", "Tiller"), new HudPrompt("Space", "Stop"),
            new HudPrompt("E", "Step ashore, by the bank"), new HudPrompt("M", "The 1900 map"),
            new HudPrompt(KeyGlyphs.RightMouse, "Look round"), new HudPrompt(KeyGlyphs.Wheel, "Zoom"),
        };

        public static readonly HudPrompt[] Ashore =
        {
            new HudPrompt("W|A|S|D", "Walk"), new HudPrompt("Shift", "Run"), new HudPrompt("E", "Step aboard, by the stern"),
            new HudPrompt("T", "Tie up or cast off"), new HudPrompt("E", "Talk, by the farmer"),
            new HudPrompt(KeyGlyphs.RightMouse, "Shoulder the rifle, held"), new HudPrompt(KeyGlyphs.LeftMouse, "Fire, shouldered"),
            new HudPrompt("M", "The 1900 map"),
            new HudPrompt(KeyGlyphs.RightMouse, "Look round"), new HudPrompt(KeyGlyphs.Wheel, "Zoom"),
        };

        public static readonly HudPrompt Open = new HudPrompt("H", "Controls");
        public static readonly HudPrompt Close = new HudPrompt("H", "Close");
    }
}
