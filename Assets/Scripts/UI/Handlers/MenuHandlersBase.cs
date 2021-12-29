using UnityEngine;

namespace Uninstructed.UI.Handlers
{
    public abstract class MenuHandlersBase : MonoBehaviour
    {
        private MenuManager menuManager;

        protected abstract GameMenu CurrentMenu { get; }

        public void Start()
        {
            menuManager = GetComponent<MenuManager>();
        }

        protected void OpenMenu(GameMenu menu)
        {
            gameObject.SetActive(false);
            menuManager.Open(CurrentMenu, menu);
        }
    }
}
