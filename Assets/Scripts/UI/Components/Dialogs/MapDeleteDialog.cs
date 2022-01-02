using System;
using System.IO;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Uninstructed.UI.Components.Dialogs
{
    public class MapDeleteDialog : DialogBase
    {
        [SerializeField]
        private Button yesButton, noButton;

        [SerializeField]
        private TMPro.TMP_Text mapInfoText;

        private GameWorldPreviewData mapPreview;


        public override void Reset()
        {
            yesButton = null;
            noButton = null;
            mapInfoText = null;
        }

        public override void Start()
        {
            yesButton.onClick.AddListener(OnClickYes);
            noButton.onClick.AddListener(OnClickNo);
        }

        public void Open(GameWorldPreviewData mapPreview)
        {
            if (!Opened)
            {
                this.mapPreview = mapPreview;
                mapInfoText.text = $"Вы действительно хотите удалить карту \r\n\"{mapPreview.MapName}\"\r\n сохранённую {mapPreview.SaveDate}?";
                Opened = true;
            }
        }

        private void OnClickYes()
        {
            if (File.Exists(mapPreview.FileName))
            {
                File.Delete(mapPreview.FileName);
                Deleted.Invoke();
            }
            
            Opened = false;
        }

        private void OnClickNo()
        {
            Opened = false;
        }

        public UnityEvent Deleted;
    }
}
