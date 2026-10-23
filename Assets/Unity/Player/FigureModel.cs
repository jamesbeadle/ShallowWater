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
        private const int NeverPosed = -1;
        private static readonly Vector3 PalmBelowTheWrist = new Vector3(0, -(float)AskewForm.PalmBelowTheWristMetres, 0);
        private readonly Dictionary<FigurePart, Transform> joints = new Dictionary<FigurePart, Transform>();
        private readonly PoseBlender blender = new PoseBlender();
        private readonly Vector3 hipsAtRest;
        private int posedInFrame = NeverPosed;

        private FigureModel(string name, Outfit outfit)
        {
            Root = new GameObject(name).transform;
            foreach (FigurePart part in Enum.GetValues(typeof(FigurePart))) joints[part] = Jointed(name, part, FigureShapes.Of(part, outfit));
            hipsAtRest = joints[FigurePart.Hips].localPosition;
            Root.gameObject.AddComponent<FigureAnimator>().Animate(this);
        }

        public Transform Root { get; }
        public bool IsPosedThisFrame => posedInFrame == Time.frameCount;

        public static FigureModel Askew()
        {
            return new FigureModel(AskewName, Outfit.Askew);
        }

        public static FigureModel Farmer()
        {
            return new FigureModel(FarmerName, Outfit.Farmer);
        }

        public Transform JointOf(FigurePart part)
        {
            return joints[part];
        }

        public Vector3 PalmOf(FigurePart hand)
        {
            var wrist = joints[hand];
            return wrist.TransformPoint(PalmBelowTheWrist);
        }

        public void Pose(FigurePose target)
        {
            var shown = blender.Towards(target, Time.time);
            foreach (var part in joints.Keys) joints[part].localRotation = JointRotations.Of(part, shown.Of(part));
            joints[FigurePart.Hips].localPosition = hipsAtRest + WorldVectors.Of(shown.HipsOffset);
            posedInFrame = Time.frameCount;
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
