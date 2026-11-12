using ShallowWater.Unity.Looks;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Title
{
    public sealed class TitleBackdrop : MonoBehaviour
    {
        private Material dusk;
        private VisualElement brand;

        public void Wear(VisualElement brandRoot)
        {
            brand = brandRoot;
            brand.RegisterCallback<CustomStyleResolvedEvent>(TakeTheTones);
        }

        private void Awake()
        {
            dusk = DuskLook.Painted();
        }

        private void TakeTheTones(CustomStyleResolvedEvent resolved)
        {
            DuskLook.Tint(dusk, resolved.customStyle);
        }

        [ImageEffectTransformsToLDR]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, dusk);
        }

        private void OnDestroy()
        {
            brand.UnregisterCallback<CustomStyleResolvedEvent>(TakeTheTones);
            Destroy(dusk);
        }
    }
}
