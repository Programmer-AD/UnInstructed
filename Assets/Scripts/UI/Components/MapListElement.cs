using Uninstructed.Game;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class MapListElement : MonoBehaviour
    {
        [SerializeField]
        private Button startButton, deleteButton;

        [SerializeField]
        private TMPro.TMP_Text mapNameText, saveDateText;

        public MapDeleteDialog DeleteDialog { get; set; }
        public GameDirector GameDirector { get; set; }

        private GameInstancePreviewData mapPreview;
        public GameInstancePreviewData MapPreview
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
            startButton = null;
            deleteButton = null;
            mapNameText = null;
            saveDateText = null;
        }

        public void Start()
        {
            startButton.onClick.AddListener(StartClick);
            deleteButton.onClick.AddListener(DeleteClick);
        }

        private void StartClick()
        {
            GameDirector.LoadMap(mapPreview.MapName);
        }

        private void DeleteClick()
        {
            DeleteDialog.Open(mapPreview);
        }
    }
}