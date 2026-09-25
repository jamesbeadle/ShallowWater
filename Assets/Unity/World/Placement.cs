using UnityEngine;

namespace ShallowWater.Unity.World
{
    public readonly struct Placement
    {
        public Vector3 Position { get; }
        public Vector3 Size { get; }
        public Quaternion Turn { get; }

        public Placement(Vector3 position, Vector3 size, Quaternion turn)
        {
            Position = position;
            Size = size;
            Turn = turn;
        }

        public void Apply(Transform transform)
        {
            transform.position = Position;
            transform.rotation = Turn;
            transform.localScale = Size;
        }
    }
}
