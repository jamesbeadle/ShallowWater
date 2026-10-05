namespace ShallowWater.Game.Houses
{
    public readonly struct Extras
    {
        public Extras(bool hasDoorHood, bool hasDormer, bool hasOutshut)
        {
            HasDoorHood = hasDoorHood;
            HasDormer = hasDormer;
            HasOutshut = hasOutshut;
        }

        public bool HasDoorHood { get; }
        public bool HasDormer { get; }
        public bool HasOutshut { get; }
    }
}
