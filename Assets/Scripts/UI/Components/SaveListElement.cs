using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Saving.Models;
using Uninstructed.Game;
using Uninstructed.UI.Components.Dialogs;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Uninstructed.UI.Components
{
    public class SaveListElement:MonoBehaviour
    {
        [SerializeField]
        private Button clickableBackground;

        [SerializeField]
        private TMPro.TMP_Text mapNameText, saveDateText;

        public SaverElement SaverElement { get; set; }

        private GameWorldPreviewData mapPreview;
        public GameWorldPreviewData MapPreview
        {
            get => MapPreview;
            set
            {
                mapPreview = value;
                mapNameText.text = mapPreview.MapName;
                saveDateText.text = mapPreview.SaveDate.ToString();
            }
        }

        public void Reset()
        {
            mapNameText = null;
            saveDateText = null;
            clickableBackground = null;
        }

        public void Start()
        {
            clickableBackground.onClick.AddListener(OnElementSelect);
        }

        private void OnElementSelect()
        {
            var fileName = Path.GetFileNameWithoutExtension(mapPreview.FileName);
            SaverElement.SaveFileName = fileName;
        }
    }
}
