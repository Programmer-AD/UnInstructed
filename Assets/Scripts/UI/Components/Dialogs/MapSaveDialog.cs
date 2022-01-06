using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components.Dialogs
{
    public class MapSaveDialog : DialogBase
    {
        [SerializeField]
        private Button cancelButton, saveButton;

        [SerializeField]
        private GameSceneManager manager;

        [SerializeField]
        private SaverElement saverElement;

        public override void Start()
        {
            cancelButton.onClick.AddListener(OnCancelClick);
            saveButton.onClick.AddListener(OnSaveClick);
        }

        public override void Reset()
        {
            base.Reset();

            cancelButton = null;
            saveButton = null;
            saverElement = null;
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

        private void OnSaveClick()
        {
            var fileName = saverElement.SaveFileName;
            manager.GameDirector.SaveMap(fileName);
            Opened = false;
        }
    }
}
