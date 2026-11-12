using UnityEditor;
using UnityEngine;

namespace ShallowWater.Editor
{
    [InitializeOnLoad]
    public static class AudioSourceIcons
    {
        private const bool IsShown = false;

        static AudioSourceIcons()
        {
            GizmoUtility.SetIconEnabled(typeof(AudioSource), IsShown);
        }
    }
}
