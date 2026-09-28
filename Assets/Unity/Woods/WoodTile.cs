using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Ground;
using ShallowWater.Game.Woods;
using UnityEngine;

namespace ShallowWater.Unity.Woods
{
    public sealed class WoodTile
    {
        private const float NearMetres = 70f;
        private const float MiddleMetres = 320f;
        private const float FarthestMetres = 3000f;
        private readonly IReadOnlyList<Tree> trees;
        private readonly Matrix4x4[] placements;
        private readonly TreeModels models;

        public WoodTile(Bounds bounds, IReadOnlyList<Tree> trees, TreeModels models)
        {
            Bounds = bounds;
            this.trees = trees;
            this.models = models;
            placements = trees.Select(PlacementOf).ToArray();
            FarAway = Grouped(Enumerable.Repeat(DetailLevel.Far, trees.Count).ToList());
        }

        public Bounds Bounds { get; }
        public IReadOnlyList<TreePlacements> FarAway { get; }

        public bool IsWithinDetailOf(Vector3 eye)
        {
            return Bounds.SqrDistance(eye) < MiddleMetres * MiddleMetres;
        }

        public bool IsWithinSightOf(Vector3 eye)
        {
            return Bounds.SqrDistance(eye) < FarthestMetres * FarthestMetres;
        }

        public IReadOnlyList<TreePlacements> SeenFrom(Vector3 eye)
        {
            var levels = placements.Select(placement => LevelAt(Vector3.Distance(eye, placement.GetPosition()))).ToList();
            return Grouped(levels);
        }

        private static DetailLevel LevelAt(float distance)
        {
            if (distance < NearMetres) return DetailLevel.Near;
            if (distance < MiddleMetres) return DetailLevel.Middle;
            return DetailLevel.Far;
        }

        private IReadOnlyList<TreePlacements> Grouped(IReadOnlyList<DetailLevel> levels)
        {
            var groups = Enumerable.Range(0, trees.Count).GroupBy(tree => (model: models.For(trees[tree].Form, levels[tree]), level: levels[tree]));
            return groups.Select(Placed).ToList();
        }

        private TreePlacements Placed(IGrouping<(TreeModel model, DetailLevel level), int> members)
        {
            var shown = members.Key;
            var isCastingShadows = shown.level != DetailLevel.Far;
            return new TreePlacements(shown.model, members.Select(member => placements[member]).ToArray(), isCastingShadows);
        }

        private static Matrix4x4 PlacementOf(Tree tree)
        {
            var foot = new Vector3((float)tree.East, (float)Heights.GroundMetres, (float)tree.North);
            var turn = Quaternion.Euler(0, (float)tree.TurnRadians * Mathf.Rad2Deg, 0);
            return Matrix4x4.TRS(foot, turn, Vector3.one * (float)tree.Scale);
        }
    }
}
