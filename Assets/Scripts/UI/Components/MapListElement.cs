using System.Collections;
using System.Collections.Generic;
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

        public string MapName
        {
            get => mapNameText.text;
            set => mapNameText.text = value;
        }

        public string SaveDate
        {
            get => saveDateText.text;
            set => saveDateText.text = value;
        }

        public string MapFileName { get; set; }

        public void Reset()
        {
            startButton = null;
            deleteButton = null;
            mapNameText = null;
        }
    }
}