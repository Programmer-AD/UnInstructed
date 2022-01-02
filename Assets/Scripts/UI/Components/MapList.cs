using System;
using System.Collections.Generic;
using Uninstructed.Game;
using Uninstructed.UI.Components.Dialogs;
using UnityEngine;
using UnityEngine.UI;

namespace Uninstructed.UI.Components
{
    public class MapList : MonoBehaviour
    {
        [SerializeField]
        private float refreshTime;

        [SerializeField]
        private MapListElement elementPrefab;

        public MapDeleteDialog DeleteDialog;

        private ScrollRect scrollRect;
        private GameDirector gameDirector;
        private float elapsedTime;
        private bool started = false;

        private List<MapListElement> listElements;

        public void Reset()
        {
            refreshTime = 5;
            elementPrefab = null;
        }

        public void Start()
        {
            if (started) return;
            gameDirector = FindObjectOfType<GameDirector>();
            scrollRect = GetComponent<ScrollRect>();
            elapsedTime = refreshTime;
            listElements = new();
            started = true;
        }

        public void OnEnable()
        {
            Start();
            RefreshList();
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

        public void OnElementDelete()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            var listContent = scrollRect.content;

            var previews = gameDirector.MapFileIO.GetPreviewList();

            var toAdd = previews.Length - listElements.Count;
            for (int i = 0; i < toAdd; i++)
            {
                var element = Instantiate(elementPrefab, listContent);
                element.DeleteDialog = DeleteDialog;
                element.GameDirector = gameDirector;
                listElements.Add(element);
            }

            var elements = listElements.GetEnumerator();
            foreach (var preview in previews)
            {
                elements.MoveNext();
                var element = elements.Current;
                element.MapPreview = preview;
                element.gameObject.SetActive(true);
            }

            while (elements.MoveNext())
            {
                var element = elements.Current;
                element.gameObject.SetActive(false);
            }
        }
    }
}
