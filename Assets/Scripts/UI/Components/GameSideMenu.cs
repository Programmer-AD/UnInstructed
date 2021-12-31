using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class GameSideMenu : MonoBehaviour
    {
        [SerializeField]
        private Button menuButton;

        [SerializeField]
        private Image sidePanel;

        private GameDirector director;

        private bool Opened => sidePanel.gameObject.activeSelf;

        public void Reset()
        {
            menuButton = null;
            sidePanel = null;
        }

        public void Start()
        {
            SetOpen(false);
            director = FindObjectOfType<GameDirector>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetOpen(!Opened);
            }
        }

        public void HandleOpen() => SetOpen(true);
        public void HandleClose() => SetOpen(false);

        private void SetOpen(bool opened)
        {
            sidePanel.gameObject.SetActive(opened);
            menuButton.gameObject.SetActive(!opened);
        }

        public void HandleStart()
        {

        }

        public void HandleSave()
        {
            director.SaveMap("test");
        }

        public void HandleExit()
        {
            director.LoadMenus();
        }
    }
}
