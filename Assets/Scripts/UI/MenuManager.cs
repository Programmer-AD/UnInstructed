using System;
using System.Collections.Generic;
using UnityEngine;

namespace Uninstructed.UI
{
    public class MenuManager : MonoBehaviour
    {
        private readonly IDictionary<GameMenu, GameObject> menus;

        public GameObject MenuContainer;

        public MenuManager()
        {
            menus = new Dictionary<GameMenu, GameObject>();
        }

        public void Start()
        {
            LoadMenus();
            menus[GameMenu.MainMenu].SetActive(true);
        }

        public void Open(GameMenu current, GameMenu menu)
        {
            if (current != menu)
            {
                menus[current].SetActive(false);
                menus[menu].SetActive(true);
            }
        }

        private void LoadMenus()
        {
            menus.Clear();
            foreach (Transform childTransform in MenuContainer.transform)
            {
                var child = childTransform.gameObject;
                string childName = child.name;
                if (Enum.TryParse(childName, out GameMenu menu))
                {
                    menus.Add(menu, child);
                    child.SetActive(false);
                }
            }
        }
    }
}
