using UnityEngine;

namespace ShallowWater.Unity.Looks
{
    public static class PeopleLooks
    {
        private const float Sooted = 0.25f;
        private const float WornIn = 0.15f;
        private const float Clean = 0;

        public static Material Coat() => WeatheredLook.Aboard(PeoplePalette.Coat, Weathering.Cloth, Sooted);
        public static Material Trousers() => WeatheredLook.Aboard(PeoplePalette.Trousers, Weathering.Cloth, Sooted);
        public static Material Shirt() => WeatheredLook.Aboard(PeoplePalette.Shirt, Weathering.Cloth, WornIn);
        public static Material Buttons() => WeatheredLook.Aboard(PeoplePalette.Buttons, Weathering.Leather, Clean);
        public static Material Scarf() => WeatheredLook.Aboard(PeoplePalette.Scarf, Weathering.Cloth, WornIn);
        public static Material Skin() => WeatheredLook.Aboard(PeoplePalette.Skin, Weathering.Complexion, Clean);
        public static Material Stubble() => WeatheredLook.Aboard(PeoplePalette.Stubble, Weathering.Cloth, Clean);
        public static Material Hair() => WeatheredLook.Aboard(PeoplePalette.Hair, Weathering.Cloth, Clean);
        public static Material Eyes() => WeatheredLook.Aboard(PeoplePalette.Eyes, Weathering.Glass, Clean);
        public static Material Cap() => WeatheredLook.Aboard(PeoplePalette.Cap, Weathering.Cloth, Sooted);
        public static Material Boots() => WeatheredLook.Aboard(PeoplePalette.Boots, Weathering.Leather, WornIn);
    }
}
