using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class Notifications : MonoBehaviour
    {
        [SerializeField]
        private Image pauseNotify, connectNotify, initNotify, startNotify;

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
                var showPause = sceneManager.GameDirector.Paused;
                pauseNotify.gameObject.SetActive(showPause);

                var playerController = sceneManager.GameDirector.PlayerController;

                var showConnect = showPause && !playerController.Working;
                connectNotify.gameObject.SetActive(showConnect);

                var showInit = showPause && !showConnect && !playerController.Inited;
                initNotify.gameObject.SetActive(showInit);

                var showStart = showPause && !showInit && !playerController.Started;
                startNotify.gameObject.SetActive(showStart);
            }
        }
    }
}
