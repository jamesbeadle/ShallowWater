using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public static class WoodTiles
    {
        private const float TileMetres = 200f;
        private const float CrownReachMetres = 14f;
        private const float TallestTreeMetres = 30f;

        public static List<WoodTile> Laid(IReadOnlyList<Tree> trees, TreeModels models)
        {
            var groups = trees.GroupBy(TileOf);
            return groups.Select(group => new WoodTile(BoundsOf(group.Key), group.ToList(), models)).ToList();
        }

        private static Vector2Int TileOf(Tree tree)
        {
            return new Vector2Int(Mathf.FloorToInt((float)tree.East / TileMetres), Mathf.FloorToInt((float)tree.North / TileMetres));
        }

        private static Bounds BoundsOf(Vector2Int tile)
        {
            var east = tile.x;
            var north = tile.y;
            var ground = (float)Heights.GroundMetres;
            var centre = new Vector3((east + 0.5f) * TileMetres, ground + TallestTreeMetres / 2, (north + 0.5f) * TileMetres);
            var size = new Vector3(TileMetres + 2 * CrownReachMetres, TallestTreeMetres, TileMetres + 2 * CrownReachMetres);
            return new Bounds(centre, size);
        }
    }
}
