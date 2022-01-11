using System;
using Uninstructed.Game;
using UnityEngine;

namespace Uninstructed.UI
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed, scaleSpeed, minScale, maxScale;

        private GameDirector director;
        private new Camera camera;

        private bool binded;

        public void Reset()
        {
            moveSpeed = 0.3f;
            scaleSpeed = 1;
            minScale = 1;
            maxScale = 12;
        }

        public void Start()
        {
            camera = GetComponent<Camera>();
            director = FindObjectOfType<GameDirector>();
            binded = false;
        }

        public void Update()
        {
            if (director.LoadFinished && director.World != null && Application.isFocused)
            {
                var wheelScroll = Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
                if (wheelScroll != 0)
                {
                    var scale = camera.orthographicSize;
                    scale = Math.Clamp(scale - wheelScroll, minScale, maxScale);
                    camera.orthographicSize = scale;
                }

                if (Input.GetMouseButton(1))
                {
                    var dx = Input.GetAxis("Mouse X") * moveSpeed;
                    var dy = Input.GetAxis("Mouse Y") * moveSpeed;
                    camera.transform.Translate(-dx, -dy, 0);
                }

                if (binded || Input.GetKeyDown(KeyCode.Space))
                {
                    MoveToPlayer();
                }

                if (Input.GetKeyDown(KeyCode.B))
                {
                    binded = !binded;
                }
            }
        }

        public void MoveToPlayer()
        {
            var playerPosition = director.World.Player.transform.position;
            var cameraPosition = camera.transform.position;
            camera.transform.position = new Vector3(playerPosition.x, playerPosition.y, cameraPosition.z);
        }
    }
}
