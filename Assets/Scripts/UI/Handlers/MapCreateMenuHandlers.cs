using Uninstructed.Game;
using Uninstructed.UI.Components;
using UnityEngine;

namespace Uninstructed.UI.Handlers
{
    public class MapCreateMenuHandlers : MenuHandlersBase
    {
        [SerializeField]
        private MapCreateConfiguration createConfiguration;

        private GameDirector director;

        protected override GameMenu CurrentMenu => GameMenu.MapCreateMenu;

        public override void Start()
        {
            base.Start();

            director = FindObjectOfType<GameDirector>();
        }

        public override void Reset()
        {
            base.Reset();
            createConfiguration = null;
        }

        public void HandleCreate()
        {
            var settings = createConfiguration.GetConfiguration();
            if (settings != null)
            {
                director.GenerateMap(settings);
            }
        }

        public void HandleBack()
        {
            OpenMenu(GameMenu.MapMainMenu);
        }
    }
}
