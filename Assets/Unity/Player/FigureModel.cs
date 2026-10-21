using System;
using System.Collections.Generic;
using ShallowWater.Game.People;
using ShallowWater.Game.Shapes;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class FigureModel
    {
        private const string AskewName = "Askew";
        private const string FarmerName = "Farmer";
        private readonly Dictionary<FigurePart, Transform> joints = new Dictionary<FigurePart, Transform>();
        private readonly Vector3 hips;

        private FigureModel(string name, Func<FigurePart, SurfaceShapes> shapes)
        {
            Root = new GameObject(name).transform;
            foreach (FigurePart part in Enum.GetValues(typeof(FigurePart))) joints[part] = Jointed(name, part, shapes(part));
            hips = joints[FigurePart.Body].localPosition;
        }

        public Transform Root { get; }

        public static FigureModel Askew()
        {
            return new FigureModel(AskewName, AskewShapes.Of);
        }

        public static FigureModel Farmer()
        {
            return new FigureModel(FarmerName, FarmerShapes.Of);
        }

        public Transform JointOf(FigurePart part)
        {
            return joints[part];
        }

        public void Pose(GaitPose pose)
        {
            foreach (var swing in pose.SwingRadians) joints[swing.Key].localRotation = Hanging(swing.Value, pose.ReachOf(swing.Key));
            var body = joints[FigurePart.Body];
            body.localPosition = hips + Vector3.up * (float)pose.BobMetres + Vector3.right * (float)pose.StepAcrossMetres;
            body.localRotation = Standing(pose.LeanRadians, pose.SwayRadians);
        }

        private static Quaternion Hanging(double swingRadians, double reachRadians)
        {
            var swungForward = Quaternion.AngleAxis(-(float)swingRadians * Mathf.Rad2Deg, Vector3.right);
            var reachedToTheRight = Quaternion.AngleAxis((float)reachRadians * Mathf.Rad2Deg, Vector3.forward);
            return reachedToTheRight * swungForward;
        }

        private static Quaternion Standing(double leanRadians, double swayRadians)
        {
            var leantForward = Quaternion.AngleAxis((float)leanRadians * Mathf.Rad2Deg, Vector3.right);
            var swayedToTheRight = Quaternion.AngleAxis(-(float)swayRadians * Mathf.Rad2Deg, Vector3.forward);
            return swayedToTheRight * leantForward;
        }

        private Transform Jointed(string name, FigurePart part, SurfaceShapes shapes)
        {
            var joint = new GameObject(part.ToString()).transform;
            var hasParent = FigureJoints.Parents.TryGetValue(part, out var parent);
            joint.SetParent(hasParent ? joints[parent] : Root, false);
            joint.localPosition = WorldVectors.Of(FigureJoints.FromTheParent[part]);
            foreach (var piece in ShapeMeshes.BuildEach(name, shapes))
            {
                var placement = piece.transform;
                placement.SetParent(joint, false);
            }
            return joint;
        }
    }
}
