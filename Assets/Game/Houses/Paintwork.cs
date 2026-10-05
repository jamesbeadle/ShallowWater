using System;

namespace ShallowWater.Game.Houses
{
    public readonly struct Paintwork
    {
        public Paintwork(int frames, int door)
        {
            Frames = frames;
            Door = door;
        }

        public int Frames { get; }
        public int Door { get; }

        public static Paintwork Chosen(Random random)
        {
            return new Paintwork(random.Next(Glazing.FramePaints), random.Next(Glazing.DoorPaints));
        }
    }
}
