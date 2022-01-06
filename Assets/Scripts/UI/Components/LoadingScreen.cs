using UnityEngine;

namespace Uninstructed.UI.Components
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private ProgressShower progressShower;

        [SerializeField]
        private GameObject menus;

        public bool Opened
        {
            get => gameObject.activeSelf;
            private set
            {
                gameObject.SetActive(value);
                if (menus != null)
                {
                    menus.SetActive(!value);
                }
            }
        }

        public void Reset()
        {
            progressShower = null;
            menus = null;
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
