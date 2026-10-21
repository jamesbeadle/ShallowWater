using ShallowWater.Game.Boat;
using UnityEngine;

namespace ShallowWater.Unity.Helm
{
    public sealed class TillerSwing : MonoBehaviour
    {
        private const float Level = 0;

        private BoatController helm;

        public void Under(BoatController boatHelm)
        {
            helm = boatHelm;
        }

        private void Update()
        {
            if (helm == null) return;
            var turned = Tiller.TurnedRadians(helm.RudderShare);
            transform.localRotation = Quaternion.Euler(Level, (float)turned * Mathf.Rad2Deg, Level);
        }
    }
}
