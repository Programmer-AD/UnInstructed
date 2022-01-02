using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game;
using Uninstructed.UI.Components.Dialogs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.IO;

namespace Uninstructed.UI.Components
{
    public class SaverElement : MonoBehaviour
    {
        private static readonly char[] invalidChars = Path.GetInvalidFileNameChars();

        [SerializeField]
        private float refreshTime;

        [SerializeField]
        private SaveListElement elementPrefab;

        [SerializeField]
        private TMP_Text errorText;

        [SerializeField]
        private TMP_InputField nameInput;

        [SerializeField]
        private Button saveButton;

        private ScrollRect scrollRect;
        private GameDirector gameDirector;
        private float elapsedTime;

        public string SaveFileName
        {
            get => nameInput.text;
            set
            {
                nameInput.text = value;
            }
        }

        public void Reset()
        {
            refreshTime = 5;
            elementPrefab = null;
            nameInput = null;
            errorText = null;
            saveButton = null;
        }

        public void Start()
        {
            gameDirector = FindObjectOfType<GameDirector>();
            scrollRect = GetComponent<ScrollRect>();
            elapsedTime = refreshTime;
            SaveFileName = "";

            nameInput.onValueChanged.AddListener(OnInputValueChanged);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= refreshTime)
            {
                elapsedTime -= refreshTime;
                RefreshList();
            }
        }

        public void OnEnable()
        {
            try
            {
                RefreshList();
                SaveFileName = gameDirector.MapFileName;
            }catch (Exception) { }
        }

        private void RefreshList()
        {
            var listContent = scrollRect.content;
            foreach (Transform item in listContent)
            {
                Destroy(item.gameObject);
            }

            var previews = gameDirector.MapFileIO.GetPreviewList();
            foreach (var preview in previews)
            {
                var element = Instantiate(elementPrefab, listContent);
                element.MapPreview = preview;
                element.SaverElement = this;
            }
        }

        private void OnInputValueChanged(string fileName)
        {
            var errorBuilder = new StringBuilder();
            var hasInvalidChars = invalidChars.Any(x => fileName.Contains(x));

            if (hasInvalidChars)
            {
                errorBuilder.AppendLine("Имя файла содержит недопустимые символы!")
                    .AppendLine("Недопустимые символы: \"<>|:*?\\/");
                saveButton.enabled = false;
            }
            else
            {
                saveButton.enabled = true;
                var filePath = gameDirector.MapFileIO.GetSavePath(fileName);
                if (File.Exists(filePath))
                {
                    errorBuilder.AppendLine("Файл с таким именем уже существует, в случаи сохранения он будет перезаписан!");
                    try
                    {
                        var preview = gameDirector.MapFileIO.LoadPreview(filePath);
                        errorBuilder.AppendLine($"Информация о сохранении: \"{preview.MapName}\"[{preview.SaveDate}].");
                    }
                    catch (Exception)
                    {
                        errorBuilder.AppendLine("Не удалось считать данные из этого файла!");
                    }
                    errorText.text = errorBuilder.ToString();
                }
            }

            errorText.text = errorBuilder.ToString();
        }
    }
}
