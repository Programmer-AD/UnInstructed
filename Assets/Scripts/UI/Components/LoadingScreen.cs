using UnityEngine;

namespace Uninstructed.UI.Components
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_Text screenText;

        [SerializeField]
        private ProgressShower progressShower;

        public string Text
        {
            get => screenText.text;
            set => screenText.text = value;
        }

        public bool Opened
        {
            get => gameObject.activeSelf;
            private set
            {
                gameObject.SetActive(value);
            }
        }

        public void Reset()
        {
            progressShower = null;
            screenText = null;
        }

        public void SetProgress(float progress)
        {
            progressShower.SetProgress(progress);
        }

        public void Open()
        {
            progressShower.SetProgress(0);
            Opened = true;
        }

        public void Close()
        {
            Opened = false;
        }
    }
}
