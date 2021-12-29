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
