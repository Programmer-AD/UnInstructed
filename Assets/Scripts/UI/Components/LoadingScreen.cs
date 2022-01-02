using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Uninstructed.UI.Components
{
    public class LoadingScreen:MonoBehaviour
    {
        [SerializeField]
        private ProgressShower progressShower;

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
        }

        public void SetProgress(float progress)
        {
            progressShower.SetProgress(progress);
        }

        public void Open()
        {
            Opened = true;
        }

        public void Close()
        {
            Opened = false;
        }
    }
}
