using System.IO;
using UnityEngine;

namespace Uninstructed.UI.Handlers
{
    public class InfoMenuHandlers : MenuHandlersBase
    {
        protected override GameMenu CurrentMenu => GameMenu.InfoMenu;

        public void BackHandler()
        {
            OpenMenu(GameMenu.MainMenu);
        }

        public void InfoHandler()
        {
            var relativePath = Path.Combine(Application.dataPath, "Docs");
            var fullPath = Path.GetFullPath(relativePath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            var url = $"file:///{fullPath}";
            Application.OpenURL(url);

        }
    }
}
