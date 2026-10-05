namespace ShallowWater.Game.Fields
{
    public readonly struct CropShare
    {
        public CropShare(Crop crop, double weight)
        {
            Crop = crop;
            Weight = weight;
        }

        public Crop Crop { get; }
        public double Weight { get; }
    }
}
