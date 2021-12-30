using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Uninstructed.Game.Mapping;

namespace Uninstructed.UI.Components
{
    public class MapCreateConfiguration : MonoBehaviour
    {
        private readonly (string name, int width, int height)[] sizes = new[]
        {
            ("Ìàëåíüêàÿ", 30, 30),
            ("Ñğåäíÿÿ", 60, 60),
            ("Áîëüøàÿ", 90, 90),
        };

        [SerializeField]
        private TMP_InputField mapNameInput;

        [SerializeField]
        private TMP_Text mapNameErrorText;

        [SerializeField]
        private TMP_Dropdown mapSizeDropdown;

        public void Start()
        {
            foreach (var size in sizes)
            {
                var optionText = $"{size.name} ({size.width}*{size.height})";
                var optionData = new TMP_Dropdown.OptionData(optionText);
                mapSizeDropdown.options.Add(optionData);
            }
            mapSizeDropdown.value = sizes.Length / 2;
            mapSizeDropdown.RefreshShownValue();

            mapNameErrorText.gameObject.SetActive(false);
        }

        public void Reset()
        {
            mapNameInput = null;
            mapNameErrorText = null;
            mapSizeDropdown = null;
        }

        public GenerationSettings GetConfiguration()
        {
            var settings = new GenerationSettings();

            if (!CheckMapName(out settings.MapName))
            {
                return null;
            }

            (_, settings.Width, settings.Height) = sizes[mapSizeDropdown.value];

            return settings;
        }

        private bool CheckMapName(out string mapName)
        {
            mapName = mapNameInput.text;
            if (mapName.Length is >= 3 and <= 32)
            {
                mapNameErrorText.gameObject.SetActive(false);
                return true;
            }
            mapNameErrorText.gameObject.SetActive(true);
            return false;
        }
    }
}
