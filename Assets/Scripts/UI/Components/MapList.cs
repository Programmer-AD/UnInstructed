using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Uninstructed.Game.Saving.IO;
using Uninstructed.Game.Saving.Models;

namespace Uninstructed.UI.Components
{
    public class MapList : MonoBehaviour
    {
        [SerializeField]
        private float refreshTime;

        [SerializeField]
        private MapListElement elementPrefab;

        private ScrollRect scrollRect;

        private float elapsedTime;

        public void Reset()
        {
            refreshTime = 5;
            elementPrefab = null;
        }

        public void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            elapsedTime = refreshTime;
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= refreshTime)
            {
                elapsedTime -= refreshTime;
                RefreshList();
            }
        }

        private void RefreshList()
        {
            var listContent = scrollRect.content;
            foreach(Transform item in listContent)
            {
                Destroy(item.gameObject);
            }

            var previews = InstanceSaver.GetPreviewList();
            foreach (var preview in previews)
            {
                var element = Instantiate(elementPrefab, listContent);
                element.MapName = preview.MapName;
                element.SaveDate = preview.SaveDate.ToString();
                element.MapFileName = preview.FileName;
            }
        }
    }
}
