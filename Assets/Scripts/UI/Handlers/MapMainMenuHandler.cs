using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class MapMainMenuHandler: MonoBehaviour
    {
        public void HandleBack()
        {
            SceneManager.LoadScene(SceneNames.Main);
        }

        public void HandleDelete()
        {
            //TODO
        }

        public void HandleCreate()
        {
            SceneManager.LoadScene(SceneNames.MapCreate);
        }

        public void HandleLoad()
        {
            //TODO
        }
    }
}
