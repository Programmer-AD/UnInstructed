using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components.Dialogs
{
    public class ProgramLoadDialog : DialogBase
    {
        [SerializeField]
        private GameSceneManager sceneManager;

        [SerializeField]
        private TMP_InputField commandInput, argumentsInput;

        [SerializeField]
        private TMP_Text statusText;

        [SerializeField]
        private Button backButton, disconnectButton, loadButton;

        [SerializeField]
        private Color errorColor, successColor;

        public override void Reset()
        {
            sceneManager = null;
            commandInput = null;
            argumentsInput = null;
            backButton = null;
            loadButton = null;
            statusText = null;
            disconnectButton = null;
            errorColor = Color.black;
            successColor = Color.black;
        }

        public override void Start()
        {
            backButton.onClick.AddListener(OnClickBack);
            loadButton.onClick.AddListener(OnClickLoad);
            disconnectButton.onClick.AddListener(OnClickDisconnect);
        }

        public void Update()
        {
            if (sceneManager.GameDirector.LoadFinished)
            {
                var working = sceneManager.GameDirector.PlayerController.Working;
                loadButton.interactable = !working;
                disconnectButton.interactable = working;
            }
        }

        public void Open()
        {
            statusText.text = "";
            Opened = true;
        }

        private void OnClickBack()
        {
            Opened = false;
        }

        private void OnClickDisconnect()
        {
            sceneManager.GameDirector.PlayerController.Stop();
            statusText.color = successColor;
            statusText.text = "Программа успешно отключена!";
        }

        private void OnClickLoad()
        {
            var command = commandInput.text;
            var arguments = argumentsInput.text;
            if (sceneManager.GameDirector.PlayerController.TryStart(command, arguments))
            {
                statusText.color = successColor;
                statusText.text = "Программа успешно подключена!";
            }
            else
            {
                statusText.color = errorColor;
                statusText.text = "Ошибка подключения!\r\nПроверьте данные и повторите попытку";
            }
        }
    }
}
