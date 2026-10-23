using ShallowWater.Unity.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShallowWater.Unity.Map
{
    public sealed class PeriodMapToggle : MonoBehaviour
    {
        private GameObject countryside;
        private GameObject periodMap;

        public void Between(GameObject countrysideSheet, GameObject periodMapSheet)
        {
            countryside = countrysideSheet;
            periodMap = periodMapSheet;
            ShowTheCountryside(true);
        }

        private void Update()
        {
            if (countryside == null) return;
            var keyboard = Keyboard.current;
            var isMapKeyPressed = Buttons.IsAnyPressedNow(keyboard?.mKey);
            if (!isMapKeyPressed) return;
            ShowTheCountryside(!countryside.activeSelf);
        }

        private void ShowTheCountryside(bool isShown)
        {
            countryside.SetActive(isShown);
            periodMap.SetActive(!isShown);
        }
    }
}
