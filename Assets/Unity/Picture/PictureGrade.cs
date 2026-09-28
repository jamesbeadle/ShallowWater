using ShallowWater.Unity.Looks;
using UnityEngine;

namespace ShallowWater.Unity.Picture
{
    public sealed class PictureGrade : MonoBehaviour
    {
        private Material grade;

        private void Awake()
        {
            grade = GradeLook.Autumn();
        }

        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, grade);
        }
    }
}
