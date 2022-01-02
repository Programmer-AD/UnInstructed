using System;
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

        public void Reset()
        {
            refreshTime = 5;
            elementPrefab = null;
        }

        public void Start()
        {
            gameDirector = FindObjectOfType<GameDirector>();
            scrollRect = GetComponent<ScrollRect>();
            elapsedTime = refreshTime;
        }

        public void OnEnable()
        {
            try
            {
                RefreshList();
            }
            catch (Exception) { }
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
            foreach (Transform item in listContent)
            {
                Destroy(item.gameObject);
            }

            var previews = gameDirector.MapFileIO.GetPreviewList();
            foreach (var preview in previews)
            {
                var element = Instantiate(elementPrefab, listContent);
                element.MapPreview = preview;
                element.DeleteDialog = DeleteDialog;
                element.GameDirector = gameDirector;
            }
        }
    }
}
