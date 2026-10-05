using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Shapes;

namespace ShallowWater.Game.Fields
{
    public static class FieldShapes
    {
        public static void Add(SurfaceShapes surfaces, Field field)
        {
            var outline = field.Outline;
            var strips = FieldStrips.Of(field);
            var outer = outline.Corners.ToList();
            var levels = strips[0].Count;
            for (var level = 0; level < levels; level++)
            {
                var widths = strips.Select(edgeStrips => edgeStrips[level].InsideMetres).ToList();
                var inner = InsetOutline.Corners(outline, widths);
                if (!InsetOutline.IsFaithful(outline, inner)) break;
                var ring = new InsetRing(outer, inner);
                for (var edge = 0; edge < outline.EdgeCount; edge++) AddStrip(surfaces, outline, edge, strips[edge][level], ring);
                outer = inner;
            }
            surfaces.Add(CropSurfaces.Of(field.Crop), Worked(outer, field.WorkedAlong));
        }

        private static void AddStrip(SurfaceShapes surfaces, ConvexOutline outline, int edge, FieldStrip strip, InsetRing ring)
        {
            if (!strip.HasWidth) return;
            var corners = ring.CornersAlong(edge);
            var direction = outline.EdgeDirection(edge);
            var start = outline.EdgeStart(edge);
            var shape = new Shape();
            foreach (var corner in corners) shape.Add(OnTheGround(corner), StripPlace(corner, start, direction, strip));
            shape.AddTriangle(0, 1, 2);
            shape.AddTriangle(0, 2, 3);
            surfaces.Add(strip.Surface, shape);
        }

        private static SurfacePlace StripPlace(GroundPoint corner, GroundPoint start, GroundPoint direction, FieldStrip strip)
        {
            var inward = direction.RightAngleClockwise;
            var fromTheHedge = (corner - start).Dot(inward);
            return new SurfacePlace(corner.Dot(direction), fromTheHedge - strip.CentreMetres);
        }

        private static Shape Worked(List<GroundPoint> corners, GroundPoint workedAlong)
        {
            var across = workedAlong.RightAngleClockwise;
            var shape = new Shape();
            foreach (var corner in corners) shape.Add(OnTheGround(corner), new SurfacePlace(corner.Dot(workedAlong), corner.Dot(across)));
            for (var corner = 1; corner < corners.Count - 1; corner++) shape.AddTriangle(0, corner, corner + 1);
            return shape;
        }

        private static WorldPoint OnTheGround(GroundPoint place)
        {
            return new WorldPoint(place.East, Heights.GroundMetres, place.North);
        }
    }
}
