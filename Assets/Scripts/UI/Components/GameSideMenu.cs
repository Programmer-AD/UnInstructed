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

        [SerializeField]
        private GameSceneManager sceneManager;

        private bool Opened => sidePanel.gameObject.activeSelf;

        public void Reset()
        {
            menuButton = null;
            sidePanel = null;
            sceneManager = null;
        }

        public void Start()
        {
            SetOpen(false);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !sceneManager.AnyDialogOpened)
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
            sceneManager.ProgramLoadDialog.Open();
        }

        public void HandleSave()
        {
            sceneManager.MapSaveDialog.Open();
        }

        public void HandleExit()
        {
            sceneManager.PlayExitDialog.Open();
        }
    }
}
