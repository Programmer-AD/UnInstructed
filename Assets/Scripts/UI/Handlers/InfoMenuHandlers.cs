using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.UI.Handlers
{
    public class InfoMenuHandlers : MenuHandlersBase
    {
        protected override GameMenu CurrentMenu => GameMenu.InfoMenu;

        public void BackHandler()
        {
            OpenMenu(GameMenu.MainMenu);
        }
    }
}