using System.Collections.Generic;
using System.Linq;

namespace ShallowWater.Game.Shapes
{
    public static class PiercedWall
    {
        private const double Half = 0.5;
        private const double ThinnestPanelMetres = 0.005;
        private const double AtTheFoot = 0;

        public static Shape Around(WallFace face, IReadOnlyList<Opening> openings)
        {
            var wall = new Shape();
            var edges = StripEdges(face, openings);
            for (var strip = 0; strip + 1 < edges.Count; strip++) AddStrip(wall, face, edges[strip], edges[strip + 1], openings);
            return wall;
        }

        private static List<double> StripEdges(WallFace face, IReadOnlyList<Opening> openings)
        {
            var edges = new List<double> { AtTheFoot, face.LengthMetres };
            foreach (var opening in openings)
            {
                edges.Add(opening.Left);
                edges.Add(opening.Right);
            }
            return edges.Distinct().OrderBy(edge => edge).ToList();
        }

        private static void AddStrip(Shape wall, WallFace face, double left, double right, IReadOnlyList<Opening> openings)
        {
            var middle = (left + right) * Half;
            var crossing = openings.Where(opening => opening.IsOver(middle)).OrderBy(opening => opening.Bottom);
            var bottom = AtTheFoot;
            foreach (var opening in crossing)
            {
                AddPanel(wall, face, new Opening(middle, right - left, bottom, opening.Bottom));
                bottom = opening.Top;
            }
            AddPanel(wall, face, new Opening(middle, right - left, bottom, face.HeightMetres));
        }

        private static void AddPanel(Shape wall, WallFace face, Opening panel)
        {
            var isTooThin = panel.HeightMetres < ThinnestPanelMetres || panel.WidthMetres < ThinnestPanelMetres;
            if (isTooThin) return;
            face.AddRectangle(wall, panel);
        }
    }
}
