using Uninstructed.Game;
using Uninstructed.UI.Components.Dialogs;
using UnityEngine;

namespace Uninstructed.UI
{
    public class GameSceneManager : MonoBehaviour
    {
        public ProgramLoadDialog ProgramLoadDialog;
        public MapSaveDialog MapSaveDialog;
        public PlayExitDialog PlayExitDialog;

        public GameDirector GameDirector { get; private set; }

        public void Start()
        {
            GameDirector = FindObjectOfType<GameDirector>();
        }

        public bool AnyDialogOpened
            => MapSaveDialog.Opened || PlayExitDialog.Opened || ProgramLoadDialog.Opened;
    }
}
