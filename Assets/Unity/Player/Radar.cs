using ShallowWater.Unity.Looks;
using ShallowWater.Unity.Map;
using ShallowWater.Unity.World;
using UnityEngine;

namespace ShallowWater.Unity.Player
{
    public sealed class Radar : MonoBehaviour
    {
        private const float ReachMetres = 400f;
        private const string North = "N";
        private const float NorthMarkShare = 0.16f;
        private const float FontToMark = 0.5f;
        private const float RimShare = 0.48f;

        private PrintedSheet map;
        private OrbitCamera view;
        private Material face;
        private GUIStyle northStyle;

        public void Over(PrintedSheet periodMap, OrbitCamera camera)
        {
            map = periodMap;
            view = camera;
            face = RadarLook.Made(map.Print, map.ReachOf(ReachMetres));
        }

        private void OnGUI()
        {
            if (face == null) return;
            var followed = view.Target;
            var guiEvent = Event.current;
            var isPainting = guiEvent.type == EventType.Repaint;
            if (followed == null || !isPainting) return;
            var heading = view.YawDegrees;
            var facing = followed.eulerAngles;
            var radar = HudLayout.RadarPlace;
            var pointing = facing.y - heading;
            RadarLook.Turn(face, map.PlaceOf(followed.position), heading, pointing);
            Graphics.DrawTexture(radar, map.Print, face);
            MarkNorth(radar, heading);
        }

        private void MarkNorth(Rect radar, float headingDegrees)
        {
            var size = radar.width * NorthMarkShare;
            northStyle = northStyle ?? HudStyle.Caps((int)(size * FontToMark), TextAnchor.MiddleCenter);
            var towardsNorth = -headingDegrees * Mathf.Deg2Rad;
            var onTheRim = new Vector2(Mathf.Sin(towardsNorth), -Mathf.Cos(towardsNorth)) * radar.width * RimShare;
            var centre = radar.center + onTheRim;
            var mark = new Rect(centre.x - size / 2, centre.y - size / 2, size, size);
            GUI.color = HudInk.Rim;
            GUI.DrawTexture(mark, HudTextures.Disc);
            GUI.color = HudInk.Unchanged;
            GUI.Label(mark, North, northStyle);
        }
    }
}
