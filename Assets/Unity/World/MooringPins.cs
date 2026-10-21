using System.Collections.Generic;
using ShallowWater.Game.Mooring;
using ShallowWater.Unity.Map;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class MooringPins
    {
        private const string PinName = "Mooring pin";
        private readonly Dictionary<MooringLine, GameObject> knocked = new Dictionary<MooringLine, GameObject>();

        public void Show(Mooring mooring, MooringLine line)
        {
            var isKnockedIn = knocked.ContainsKey(line);
            var isWanted = IsWantedFor(mooring, line);
            if (isWanted == isKnockedIn) return;
            if (!isWanted) PullOut(line);
            if (!isWanted) return;
            var laid = mooring.LaidAt(line);
            Knock(line, laid);
        }

        private static bool IsWantedFor(Mooring mooring, MooringLine line)
        {
            if (!mooring.IsLaid(line)) return false;
            var laid = mooring.LaidAt(line);
            return laid.IsAtAPin;
        }

        private void Knock(MooringLine line, LaidLine laid)
        {
            var pin = new GameObject(PinName);
            var placement = pin.transform;
            foreach (var piece in ShapeMeshes.BuildEach(PinName, MooringPin.Knocked(laid.Post))) piece.transform.SetParent(placement, false);
            knocked[line] = pin;
        }

        private void PullOut(MooringLine line)
        {
            var pin = knocked[line];
            foreach (var filter in pin.GetComponentsInChildren<MeshFilter>()) Object.Destroy(filter.sharedMesh);
            Object.Destroy(pin);
            knocked.Remove(line);
        }
    }
}
