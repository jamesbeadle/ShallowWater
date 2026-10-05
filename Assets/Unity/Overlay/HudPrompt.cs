namespace ShallowWater.Unity.Overlay
{
    public sealed class HudPrompt
    {
        public HudPrompt(string keys, string words)
        {
            Keys = keys;
            Words = words;
        }

        public string Keys { get; }
        public string Words { get; }
    }
}
