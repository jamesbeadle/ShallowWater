using ShallowWater.Game.Ground;
using ShallowWater.Game.Pound;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public static class FazeleyChain
    {
        private static readonly Color Iron = new Color(0.14f, 0.14f, 0.15f);
        private const double IntoTheArmMetres = 8;
        private const float PostHeightMetres = 1.4f;
        private const float PostWidthMetres = 0.25f;
        private const float PostsBackFromTheWaterMetres = 0.5f;
        private const float SagMetres = 0.8f;
        private const int LinkCount = 30;
        private const float LinkThicknessMetres = 0.07f;
        private const float LinkShareOfItsSpan = 0.8f;

        public static void Hang()
        {
            if (!MapFiles.Has(MapLayers.Canal)) return;
            var junction = LandmarkPlaces.Of(Landmark.FazeleyJunction);
            var arm = new Centreline(CanalArms.StartingNearest(MapLines.Read(MapLayers.Canal), junction));
            var paint = Paint.Of(Iron);
            var reach = PoundLimits.ChannelHalfWidthMetres + PostsBackFromTheWaterMetres;
            var eastPostTop = Post(arm, -reach, paint);
            var westPostTop = Post(arm, reach, paint);
            var links = ChainSag.Between(eastPostTop, westPostTop, SagMetres, LinkCount);
            for (var link = 0; link < LinkCount; link++) Link(links[link], links[link + 1], paint);
        }

        private static WorldPoint Post(Centreline arm, double across, Material paint)
        {
            var ground = arm.Offset(IntoTheArmMetres, across);
            var middle = Heights.BankTopMetres + PostHeightMetres / 2;
            var position = new Vector3((float)ground.East, (float)middle, (float)ground.North);
            var size = new Vector3(PostWidthMetres, PostHeightMetres / Primitives.CylinderHeightUnits, PostWidthMetres);
            Blocks.Place(PrimitiveType.Cylinder, "Chain post", paint, new Placement(position, size, Quaternion.identity));
            return new WorldPoint(position.x, Heights.BankTopMetres + PostHeightMetres, position.z);
        }

        private static void Link(WorldPoint from, WorldPoint to, Material paint)
        {
            var start = WorldVectors.Of(from);
            var span = WorldVectors.Of(to) - start;
            var turn = Quaternion.FromToRotation(Vector3.up, span);
            var size = new Vector3(LinkThicknessMetres, span.magnitude * LinkShareOfItsSpan / Primitives.CylinderHeightUnits, LinkThicknessMetres);
            Blocks.Place(PrimitiveType.Cylinder, "Chain link", paint, new Placement(start + span / 2, size, turn));
        }
    }
}
