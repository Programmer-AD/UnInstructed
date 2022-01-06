using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components.Dialogs
{
    public class PlayExitDialog : DialogBase
    {
        [SerializeField]
        private Button cancelButton, exitButton, saveButton;

        [SerializeField]
        private GameSceneManager manager;

        public override void Start()
        {
            cancelButton.onClick.AddListener(OnCancelClick);
            exitButton.onClick.AddListener(OnExitClick);
            saveButton.onClick.AddListener(OnSaveClick);
        }

        public override void Reset()
        {
            base.Reset();

            cancelButton = null;
            exitButton = null;
            saveButton = null;
            manager = null;
        }

        public void Open()
        {
            Opened = true;
        }

        private void OnCancelClick()
        {
            Opened = false;
        }

        private void OnExitClick()
        {
            manager.GameDirector.LoadMenus();
            Opened = false;
        }

        private void OnSaveClick()
        {
            manager.MapSaveDialog.Open();
        }
    }
}
