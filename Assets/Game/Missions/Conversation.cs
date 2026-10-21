using System.Collections.Generic;

namespace ShallowWater.Game.Missions
{
    public sealed class Conversation
    {
        private IReadOnlyList<string> lines = new string[0];
        private int spoken;

        public string Speaker { get; private set; } = string.Empty;
        public bool IsTalking => spoken < lines.Count;
        public string Line => IsTalking ? lines[spoken] : string.Empty;
        public bool IsOnTheLastLine => IsTalking && spoken == lines.Count - 1;

        public void Begin(string speaker, IReadOnlyList<string> said)
        {
            Speaker = speaker;
            lines = said;
            spoken = 0;
        }

        public void Next()
        {
            if (IsTalking) spoken++;
        }
    }
}
