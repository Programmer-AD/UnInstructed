using UnityEngine;

namespace Uninstructed.UI.Handlers
{
    public class MainMenuHandlers : MenuHandlersBase
    {
        protected override GameMenu CurrentMenu => GameMenu.MainMenu;

        public void HandlePlay()
        {
            OpenMenu(GameMenu.MapMainMenu);
        }

        public void HandleInformation()
        {
            OpenMenu(GameMenu.InfoMenu);
        }

        public void HandleExit()
        {
            Application.Quit();
        }
    }
}
