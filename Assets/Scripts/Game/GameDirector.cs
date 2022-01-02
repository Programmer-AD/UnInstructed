using System;
using System.Collections;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.IO;
using Uninstructed.UI.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Uninstructed.Game
{
    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private GameWorld worldPrefab;

        public WorldFileIO MapFileIO { get; private set; }
        public GameObjectFactory GameObjectFactory { get; private set; }
        public WorldGenerator WorldGenerator { get; private set; }
        public GameWorld GameWorld { get; private set; }

        public string MapFileName { get; set; }

        public void Start()
        {
            var existingDirector = FindObjectOfType<GameDirector>();
            if (existingDirector != this && existingDirector != null)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
            MapFileIO = new WorldFileIO();
            GameObjectFactory = GetComponent<GameObjectFactory>();
            WorldGenerator = new WorldGenerator(GameObjectFactory);
        }

        public void Reset()
        {
            worldPrefab = null;
        }

        public void GenerateMap(GenerationSettings settings)
        {
            LoadGameSceneAsync(() =>
            {
                GameWorld.MapName = settings.MapName;
                WorldGenerator.Generate(settings, GameWorld);
            });
        }

        public void LoadMap(string mapFileName)
        {
            LoadGameSceneAsync(() =>
            {
                MapFileName = mapFileName;
                var instanceData = MapFileIO.Load(mapFileName);
                GameWorld.Load(instanceData, GameObjectFactory);
            });
        }

        public void SaveMap(string fileName)
        {
            var instanceData = GameWorld.Save();
            var path = MapFileIO.GetSavePath(fileName);
            MapFileIO.Save(path, instanceData);
        }

        public void LoadMenus()
        {
            Destroy(GameWorld);
            SceneManager.LoadScene("MainMenu");
            GC.Collect();
        }

        private void LoadGameSceneAsync(Action onComplete)
        {
            StartCoroutine(SceneLoading(onComplete));
        }

        private IEnumerator SceneLoading(Action onComplete)
        {
            var loadingScreen = FindObjectOfType<LoadingScreen>(true);

            loadingScreen.Open();
            var sceneLoading = SceneManager.LoadSceneAsync("GameScene");
            sceneLoading.allowSceneActivation = false;

            var progress = sceneLoading.progress;
            while (!sceneLoading.isDone)
            {
                var newProgress = sceneLoading.progress;
                if (progress != newProgress)
                {
                    progress = newProgress;
                    loadingScreen.SetProgress(progress);
                }

                if (progress >= 0.9f)
                {
                    loadingScreen.Close();
                    sceneLoading.allowSceneActivation = true;
                }

                yield return new WaitForSeconds(.05f);
            }

            var buildingScreen = FindObjectOfType<LoadingScreen>(true);
            buildingScreen.Open();

            GameWorld = Instantiate(worldPrefab);
            buildingScreen.SetProgress(0.1f);

            onComplete();
            buildingScreen.SetProgress(0.85f);

            GameWorld.Init();
            buildingScreen.SetProgress(1f);
            buildingScreen.Close();
        }
    }
}
