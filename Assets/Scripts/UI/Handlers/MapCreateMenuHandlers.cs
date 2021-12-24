using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class MapCreateMenuHandlers: MenuHandlersBase
    {
        protected override GameMenu CurrentMenu => GameMenu.MapCreateMenu;

        public void HandleCreate()
        {
            //TODO
        }

        public void HandleBack()
        {
            OpenMenu(GameMenu.MapMainMenu);
        }
    }
}
