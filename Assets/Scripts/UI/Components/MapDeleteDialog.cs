using System.IO;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class MapDeleteDialog : MonoBehaviour
    {
        [SerializeField]
        private Button yesButton, noButton;

        [SerializeField]
        private TMPro.TMP_Text mapInfoText;

        private GameWorldPreviewData mapPreview;

        public bool Opened => gameObject.activeSelf;

        public void Reset()
        {
            yesButton = null;
            noButton = null;
            mapInfoText = null;
        }

        public void Start()
        {
            yesButton.onClick.AddListener(OnClickYes);
            noButton.onClick.AddListener(OnClickNo);
            gameObject.SetActive(false);
        }

        public void Open(GameWorldPreviewData mapPreview)
        {
            if (!Opened)
            {
                this.mapPreview = mapPreview;
                mapInfoText.text = $"�� ������������� ������ ������� ����� \"{mapPreview.MapName}\" ����������� {mapPreview.SaveDate}?";
                gameObject.SetActive(true);
            }
        }

        public void Close()
        {
            if (!Opened)
            {
                gameObject.SetActive(true);
            }
        }

        private void OnClickYes()
        {
            if (File.Exists(mapPreview.FileName))
            {
                File.Delete(mapPreview.FileName);
            }
            Close();
        }

        private void OnClickNo()
        {
            Close();
        }
    }
}
