using ShallowWater.Game.Ground;

namespace ShallowWater.Game.Shapes
{
    public static class LevelGround
    {
        public static Shape At(GroundRing outline, double heightMetres)
        {
            var ground = Extrusion.Roof(outline, heightMetres);
            return Subdivision.Finer(ground, GroundTriangles.LongestEdgeMetres);
        }
    }
}
