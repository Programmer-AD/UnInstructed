namespace Uninstructed.UI.Handlers
{
    public class MapMainMenuHandlers : MenuHandlersBase
    {
        protected override GameMenu CurrentMenu => GameMenu.MapMainMenu;

        public void HandleBack()
        {
            OpenMenu(GameMenu.MainMenu);
        }

        public void HandleCreate()
        {
            OpenMenu(GameMenu.MapCreateMenu);
        }
    }
}
