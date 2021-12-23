using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class MainMenuHandlers : MonoBehaviour
    {
        public void HandlePlay()
        {
            SceneManager.LoadScene(SceneNames.MapMain);
        }

        public void HandleInformation()
        {
            SceneManager.LoadScene(SceneNames.Information);
        }

        public void HandleExit()
        {
            Application.Quit();
        }
    }
}
