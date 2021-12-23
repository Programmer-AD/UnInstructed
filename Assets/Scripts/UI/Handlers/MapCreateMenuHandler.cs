using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class MapCreateMenuHandler:MonoBehaviour
    {
        public void HandleCreate()
        {
            //TODO
        }

        public void HandleBack()
        {
            SceneManager.LoadScene(SceneNames.MapMain);
        }
    }
}
