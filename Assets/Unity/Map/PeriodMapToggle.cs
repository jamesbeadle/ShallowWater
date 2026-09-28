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
            var keyboard = Keyboard.current;
            var isReady = keyboard != null && countryside != null;
            if (!isReady) return;
            var mapKey = keyboard.mKey;
            if (!mapKey.wasPressedThisFrame) return;
            ShowTheCountryside(!countryside.activeSelf);
        }

        private void ShowTheCountryside(bool isShown)
        {
            countryside.SetActive(isShown);
            periodMap.SetActive(!isShown);
        }
    }
}
