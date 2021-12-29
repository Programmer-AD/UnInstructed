namespace Uninstructed.UI.Handlers
{
    public class MapCreateMenuHandlers : MenuHandlersBase
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
