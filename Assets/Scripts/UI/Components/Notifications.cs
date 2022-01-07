using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class Notifications : MonoBehaviour
    {
        [SerializeField]
        private Image pauseNotify, connectNotify, startNotify;

        [SerializeField]
        private GameSceneManager sceneManager;

        public void Reset()
        {
            pauseNotify = null;
            connectNotify = null;
            sceneManager = null;
            startNotify = null;
        }

        public void Start()
        {
            pauseNotify.gameObject.SetActive(false);
            connectNotify.gameObject.SetActive(false);
            startNotify.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (sceneManager.GameDirector.LoadFinished)
            {
                var showPause = sceneManager.GameDirector.GameWorld.Paused;
                pauseNotify.gameObject.SetActive(showPause);

                var showConnect = showPause && !sceneManager.GameDirector.PlayerController.Working;
                connectNotify.gameObject.SetActive(showConnect);

                var showStart = showPause && !showConnect && !sceneManager.GameDirector.PlayerController.Started;
                startNotify.gameObject.SetActive(showStart);
            }
        }
    }
}
