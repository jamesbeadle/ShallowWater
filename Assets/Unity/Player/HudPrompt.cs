namespace ShallowWater.Unity.Player
{
    public readonly struct HudPrompt
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
