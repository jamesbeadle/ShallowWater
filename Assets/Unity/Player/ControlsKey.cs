using ShallowWater.Unity.Overlay;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShallowWater.Unity.Player
{
    public sealed class ControlsKey : MonoBehaviour
    {
        private const string AtTheHelm = "At the tiller";
        private const string Ashore = "Ashore";

        private ShoreLeave shore;
        private ControlsPanel panel;
        private bool isShowingAshore;

        public void Shows(VisualElement screen)
        {
            panel = new ControlsPanel(screen, ControlsRows.Open, ControlsRows.Close);
            ShowTheRows(isAshore: false);
        }

        private void Start()
        {
            shore = GetComponent<ShoreLeave>();
        }

        private void Update()
        {
            var isShowing = panel != null && shore != null;
            if (!isShowing) return;
            if (KeyInput.IsTogglingTheKey()) panel.Toggle();
            var isAshore = shore.IsAshore;
            var hasLandedOrBoarded = isAshore != isShowingAshore;
            if (hasLandedOrBoarded) ShowTheRows(isAshore);
        }

        private void ShowTheRows(bool isAshore)
        {
            isShowingAshore = isAshore;
            var heading = isAshore ? Ashore : AtTheHelm;
            var rows = isAshore ? ControlsRows.Ashore : ControlsRows.AtTheHelm;
            panel.Show(heading, rows);
        }
    }
}
