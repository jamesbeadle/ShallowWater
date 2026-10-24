using System.Collections.Generic;
using ShallowWater.Game.Guns;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public sealed class GunLocker
    {
        private const string Heading = "GUNS";
        private const string InHand = "in hand";
        private const string Slung = "slung";
        private const string StowedBelow = "stowed below";
        private const string Comma = ", ";
        private const int SpareHooks = 1;

        private readonly Dictionary<Rifle, PageEntry> hooks = new Dictionary<Rifle, PageEntry>();

        public GunLocker(VisualElement page, Rifles rifles)
        {
            var section = PageElements.Section(page, Heading);
            foreach (var rifle in rifles.All) section.Add(Hook(rifle));
            for (var hook = 0; hook < SpareHooks; hook++) section.Add(SpareHook());
        }

        public void Show(Rifles rifles, bool isAshore)
        {
            foreach (var rifle in rifles.All) Write(rifle, Where(rifles, rifle, isAshore));
        }

        private void Write(Rifle rifle, string where)
        {
            var hook = hooks[rifle];
            hook.Write(rifle.Name, rifle.Description + Comma + rifle.Provenance, where + Comma + rifle.Readiness);
        }

        private VisualElement Hook(Rifle rifle)
        {
            var sketch = rifle.HasATelescope ? Sketches.ScopedRifle() : Sketches.Rifle();
            var hook = new PageEntry(sketch);
            hooks[rifle] = hook;
            return hook.Row;
        }

        private static string Where(Rifles rifles, Rifle rifle, bool isAshore)
        {
            if (!isAshore) return StowedBelow;
            return rifle == rifles.InHand ? InHand : Slung;
        }

        private static VisualElement SpareHook()
        {
            var hook = new PageEntry(Sketches.Hook());
            hook.Fade(true);
            return hook.Row;
        }
    }
}
