using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Overlay
{
    public static class KeyGlyphs
    {
        public const string RightMouse = "Right mouse";
        public const string Wheel = "Wheel";
        private const char Between = '|';
        private const int LetterLength = 1;
        private const int FirstPlace = 0;
        private const string KeysClass = "hud-keys";
        private const string FollowingClass = "hud-glyph--after";
        private const string CapClass = "hud-cap";
        private const string WordCapClass = "hud-cap--word";

        private static readonly Dictionary<string, Func<VisualElement>> MouseGlyphs = new Dictionary<string, Func<VisualElement>>
        {
            { RightMouse, MouseGlyph.Looking },
            { Wheel, MouseGlyph.Zooming }
        };

        public static VisualElement For(string keys)
        {
            var row = OverlayElements.Block(KeysClass);
            var names = keys.Split(Between);
            for (var place = FirstPlace; place < names.Length; place++) row.Add(Glyph(names[place], place));
            return row;
        }

        private static VisualElement Glyph(string key, int place)
        {
            var isMouse = MouseGlyphs.TryGetValue(key, out var mouseGlyph);
            var glyph = isMouse ? mouseGlyph() : Cap(key);
            var isFollowing = place > FirstPlace;
            if (isFollowing) glyph.AddToClassList(FollowingClass);
            return glyph;
        }

        private static VisualElement Cap(string key)
        {
            var isWord = key.Length > LetterLength;
            if (!isWord) return OverlayElements.Words(key, CapClass);
            return OverlayElements.Words(key.ToUpperInvariant(), CapClass, WordCapClass);
        }
    }
}
