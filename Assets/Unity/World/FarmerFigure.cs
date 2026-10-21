using ShallowWater.Game.Farmyard;
using ShallowWater.Game.Ground;
using ShallowWater.Game.People;
using ShallowWater.Game.Walking;
using ShallowWater.Unity.Player;
using UnityEngine;

namespace ShallowWater.Unity.World
{
    public sealed class FarmerFigure : MonoBehaviour
    {
        private const double NoticesFromMetres = 9;
        private const float TurnPerSecond = 3f;
        private const double Still = 0;

        private Farmyard yard;
        private Transform askew;
        private float bearingDegrees;

        public static FarmerFigure Standing(Farmyard yard, Land land, Transform askew)
        {
            var figure = FigureModel.Farmer();
            figure.Pose(Gait.At(Still, Still));
            var root = figure.Root;
            var farmer = root.gameObject.AddComponent<FarmerFigure>();
            farmer.yard = yard;
            farmer.askew = askew;
            farmer.bearingDegrees = (float)yard.FarmerBearing * Mathf.Rad2Deg;
            var feet = yard.FarmerStands;
            root.position = new Vector3((float)feet.East, (float)land.HeightAt(feet), (float)feet.North);
            return farmer;
        }

        private void Update()
        {
            var ease = 1 - Mathf.Exp(-TurnPerSecond * Time.deltaTime);
            bearingDegrees = Mathf.LerpAngle(bearingDegrees, WantedBearingDegrees(), ease);
            transform.rotation = Quaternion.Euler(0, bearingDegrees, 0);
        }

        private float WantedBearingDegrees()
        {
            var position = askew.position;
            var askewFeet = new GroundPoint(position.x, position.z);
            var towardsAskew = askewFeet - yard.FarmerStands;
            var hasNoticedHim = towardsAskew.Length < NoticesFromMetres;
            var bearing = hasNoticedHim ? towardsAskew.Bearing : yard.FarmerBearing;
            return (float)bearing * Mathf.Rad2Deg;
        }
    }
}
