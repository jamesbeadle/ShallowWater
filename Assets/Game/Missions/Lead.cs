namespace ShallowWater.Game.Missions
{
    public sealed class Lead
    {
        private const string Dot = " · ";

        public Lead(string name, string pub, string place, string telephone)
        {
            Name = name;
            Pub = pub;
            Place = place;
            Telephone = telephone;
        }

        public string Name { get; }
        public string Pub { get; }
        public string Place { get; }
        public string Telephone { get; }
        public string Words => Name + Dot + Pub + ", " + Place + Dot + Telephone;
    }
}
