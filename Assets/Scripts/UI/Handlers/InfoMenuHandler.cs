using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class InfoMenuHandler : MonoBehaviour
    {
        public void BackHandler()
        {
            SceneManager.LoadScene(SceneNames.Main);
        }
    }
}
