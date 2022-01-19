using UnityEngine;

namespace Uninstructed.UI.Handlers
{
    public abstract class MenuHandlersBase : MonoBehaviour
    {
        private MenuManager menuManager;

        protected abstract GameMenu CurrentMenu { get; }

        public virtual void Start()
        {
            menuManager = GetComponent<MenuManager>();
        }

        public virtual void Reset() { }

        protected void OpenMenu(GameMenu menu)
        {
            gameObject.SetActive(false);
            menuManager.Open(CurrentMenu, menu);
        }
    }
}
