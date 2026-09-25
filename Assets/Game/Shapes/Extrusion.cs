using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Extrusion
    {
        public static Shape Walls(GroundRing footprint, double bottom, double top)
        {
            var walls = new Shape();
            var corners = footprint.Corners;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                AddWall(walls, corners[previous], corners[index], bottom, top);
            }
            return walls;
        }

        public static Shape Roof(GroundRing footprint, double top)
        {
            var roof = new Shape();
            foreach (var corner in footprint.Corners) roof.Add(new WorldPoint(corner.East, top, corner.North));
            foreach (var ear in Triangulation.OfClockwise(footprint.Corners)) roof.AddTriangle(ear[0], ear[1], ear[2]);
            return roof;
        }

        private static void AddWall(Shape walls, GroundPoint from, GroundPoint to, double bottom, double top)
        {
            var fromBottom = walls.Add(new WorldPoint(from.East, bottom, from.North));
            var toBottom = walls.Add(new WorldPoint(to.East, bottom, to.North));
            var fromTop = walls.Add(new WorldPoint(from.East, top, from.North));
            var toTop = walls.Add(new WorldPoint(to.East, top, to.North));
            walls.AddQuad(toBottom, fromBottom, toTop, fromTop);
        }
    }
}
