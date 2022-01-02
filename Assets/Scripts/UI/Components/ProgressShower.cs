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
            var escapedProgress = MathF.Round(progress * 100, 2);
            var decimalProgress = decimal.Round((decimal)escapedProgress, 2);
            progressText.text = $"{decimalProgress}%";
            progressBar.value = escapedProgress;
        }
    }
}
