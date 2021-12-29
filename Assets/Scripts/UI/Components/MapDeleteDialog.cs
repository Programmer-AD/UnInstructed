using System.Collections;
using System.Collections.Generic;
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

        private GameInstancePreviewData mapPreview;

        public bool Opened { get; private set; }

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
            Opened = false;
        }

        public void Open(GameInstancePreviewData mapPreview)
        {
            if (!Opened)
            {
                this.mapPreview = mapPreview;
                mapInfoText.text = $"Вы действительно хотите удалить карту \"{mapPreview.MapName}\" сохранённую {mapPreview.SaveDate}?";
                gameObject.SetActive(true);
                Opened = true;
            }
        }

        public void Close()
        {
            if (!Opened)
            {
                gameObject.SetActive(true);
                Opened = false;
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
