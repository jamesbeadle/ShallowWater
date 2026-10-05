using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class Extrusion
    {
        private const double AtTheFoot = 0;

        public static Shape Walls(GroundRing footprint, double bottom, double top)
        {
            var walls = new Shape();
            var rise = new Rise(bottom, top);
            var corners = footprint.Corners;
            var along = 0.0;
            for (int index = 0, previous = corners.Count - 1; index < corners.Count; previous = index++)
            {
                AddWall(walls, corners[previous], corners[index], along, rise);
                along += corners[previous].DistanceTo(corners[index]);
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

        public static Shape Floor(GroundRing footprint, double bottom)
        {
            var floor = new Shape();
            foreach (var corner in footprint.Corners) floor.Add(new WorldPoint(corner.East, bottom, corner.North));
            foreach (var ear in Triangulation.OfClockwise(footprint.Corners)) floor.AddTriangle(ear[0], ear[2], ear[1]);
            return floor;
        }

        public static void AddWall(Shape walls, GroundPoint from, GroundPoint to, double alongFrom, Rise rise)
        {
            var alongTo = alongFrom + from.DistanceTo(to);
            var fromBottom = walls.Add(new WorldPoint(from.East, rise.FootMetres, from.North), new SurfacePlace(alongFrom, AtTheFoot));
            var toBottom = walls.Add(new WorldPoint(to.East, rise.FootMetres, to.North), new SurfacePlace(alongTo, AtTheFoot));
            var fromTop = walls.Add(new WorldPoint(from.East, rise.TopMetres, from.North), new SurfacePlace(alongFrom, rise.HeightMetres));
            var toTop = walls.Add(new WorldPoint(to.East, rise.TopMetres, to.North), new SurfacePlace(alongTo, rise.HeightMetres));
            walls.AddQuad(toBottom, fromBottom, toTop, fromTop);
        }
    }
}
