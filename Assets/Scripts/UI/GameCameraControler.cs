﻿using System;
using UnityEngine;

namespace Uninstructed.UI
{
    public class GameCameraControler : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed, scaleSpeed, minScale, maxScale;

        [SerializeField]
        private GameSceneManager sceneManager;

        private new Camera camera;
        private bool startCentralized = false;

        public void Reset()
        {
            moveSpeed = 0.3f;
            scaleSpeed = 1;
            minScale = 1;
            maxScale = 12;
            sceneManager = null;
        }

        public void Start()
        {
            camera = GetComponent<Camera>();
        }

        public void Update()
        {
            if (sceneManager.GameDirector.LoadFinished && Application.isFocused)
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

                if (Input.GetKeyDown(KeyCode.Space) || !startCentralized)
                {
                    var playerPosition = sceneManager.GameDirector.World.Player.transform.position;
                    var cameraPosition = camera.transform.position;
                    camera.transform.position = new Vector3(playerPosition.x, playerPosition.y, cameraPosition.z);

                    startCentralized = true;
                }
            }
        }
    }
}
