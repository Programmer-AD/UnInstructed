using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using Uninstructed.Game;
using UnityEngine;
using UnityEngine.UI;

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

        private bool started = false;
        private List<SaveListElement> listElements;

        public string SaveFileName
        {
            get => nameInput.text;
            set => nameInput.text = value;
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
            if (started)
            {
                return;
            }

            gameDirector = FindObjectOfType<GameDirector>();
            scrollRect = GetComponent<ScrollRect>();
            elapsedTime = refreshTime;
            SaveFileName = "";
            listElements = new();

            nameInput.onValueChanged.AddListener(OnInputValueChanged);

            started = true;
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
            Start();
            SaveFileName = Path.GetFileNameWithoutExtension(gameDirector.MapFilePath);
            RefreshList();
        }

        private void RefreshList()
        {
            var listContent = scrollRect.content;

            var previews = gameDirector.MapFileIO.GetPreviewList();

            var toAdd = previews.Length - listElements.Count;
            for (var i = 0; i < toAdd; i++)
            {
                var element = Instantiate(elementPrefab, listContent);
                element.SaverElement = this;
                listElements.Add(element);
            }

            var elements = listElements.GetEnumerator();
            foreach (var preview in previews)
            {
                elements.MoveNext();
                var element = elements.Current;
                element.MapPreview = preview;
                element.gameObject.SetActive(true);
            }

            while (elements.MoveNext())
            {
                var element = elements.Current;
                element.gameObject.SetActive(false);
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
                }
            }

            errorText.text = errorBuilder.ToString();
        }
    }
}
