using System.Collections.Generic;

namespace ShallowWater.Game.Shapes
{
    public static class Sweep
    {
        public static Shape Lengthways(IReadOnlyList<IReadOnlyList<WorldPoint>> sections)
        {
            var shape = new Shape();
            foreach (var section in sections) AddSection(shape, section);
            var width = sections[0].Count;
            for (var station = 0; station + 1 < sections.Count; station++) JoinStations(shape, station * width, width);
            return shape;
        }

        private static void AddSection(Shape shape, IReadOnlyList<WorldPoint> section)
        {
            foreach (var point in section) shape.Add(point, new SurfacePlace(point.North, point.Height));
        }

        private static void JoinStations(Shape shape, int firstOfTheStation, int width)
        {
            for (var corner = firstOfTheStation; corner + 1 < firstOfTheStation + width; corner++)
            {
                shape.AddQuad(corner, corner + 1, corner + width, corner + width + 1);
            }
        }
    }
}
