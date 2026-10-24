using System.Collections.Generic;
using ShallowWater.Game.Guns;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class CartridgeBoxes
    {
        private const string Heading = "CARTRIDGES";

        private readonly Dictionary<Rifle, CartridgeBox> boxes = new Dictionary<Rifle, CartridgeBox>();

        public CartridgeBoxes(VisualElement page, Rifles rifles)
        {
            var section = PageElements.Section(page, Heading);
            foreach (var rifle in rifles.All) boxes[rifle] = new CartridgeBox(section, rifle);
        }

        public void Show(Rifles rifles)
        {
            foreach (var rifle in rifles.All) boxes[rifle].Show(rifle.Cartridges);
        }
    }
}
