using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Uninstructed.UI
{
    public class GameCameraControler : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed, scaleSpeed, minScale, maxScale;

        private new Camera camera;

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
        }

        public void Update()
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
        }
    }
}
