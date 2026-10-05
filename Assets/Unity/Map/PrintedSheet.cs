using ShallowWater.Game.Ground;
using UnityEngine;

namespace ShallowWater.Unity.Map
{
    public sealed class PrintedSheet
    {
        private readonly MapSheet sheet;

        public Texture2D Print { get; }

        public PrintedSheet(Texture2D print, MapSheet mapSheet)
        {
            Print = print;
            sheet = mapSheet;
        }

        public Vector2 PlaceOf(Vector3 position)
        {
            var across = (position.x - sheet.West) / sheet.WidthMetres;
            var up = (position.z - sheet.South) / sheet.DepthMetres;
            return new Vector2((float)across, (float)up);
        }

        public Vector2 ReachOf(float metres)
        {
            var across = metres / sheet.WidthMetres;
            var up = metres / sheet.DepthMetres;
            return new Vector2((float)across, (float)up);
        }
    }
}
