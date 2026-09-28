using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class LookShaders
    {
        public const string Weathered = "Shallow Water/Weathered";
        public const string Stone = "Shallow Water/Stone";
        public const string Brick = "Shallow Water/Brick";
        public const string Slate = "Shallow Water/Slate";
        public const string Hedge = "Shallow Water/Hedge";
        public const string Crown = "Shallow Water/Crown";
        public const string Livery = "Shallow Water/Livery";
        public const string Railway = "Shallow Water/Railway";
        public const string Fields = "Shallow Water/Fields";
        public const string Water = "Shallow Water/Water";
        public const string Sky = "Shallow Water/Sky";
        public const string Grade = "Hidden/Shallow Water/Grade";

        public static Material Made(string lookName)
        {
            var shader = Shader.Find(lookName);
            var isMissing = shader == null;
            if (isMissing) throw new MissingReferenceException($"The {lookName} shader is missing: it lives in Assets/Unity/Resources/Shaders.");
            return new Material(shader);
        }
    }
}
