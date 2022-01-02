using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class ProgressShower:MonoBehaviour
    {
        [SerializeField]
        private TMP_Text progressText;

        [SerializeField]
        private Slider progressBar;

        public void Reset()
        {
            progressText = null;
            progressBar = null;
        }

        public void SetProgress(float progress)
        {
            var decimalProgress = decimal.Round((decimal)(progress * 100), 2);
            progressText.text = $"{decimalProgress}%";
            progressBar.value = progress;
        }
    }
}
