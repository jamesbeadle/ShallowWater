using System;

namespace ShallowWater.Unity.Map
{
    [Serializable]
    public sealed class GroundRecord
    {
        public string image;
        public float west;
        public float east;
        public float south;
        public float north;
    }
}
