using System;
using System.Collections;
using System.Collections.Generic;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Player;
using Uninstructed.Game.Saving.IO;
using Uninstructed.UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace Uninstructed.Game
{
    public class GameDirector : MonoBehaviour
    {
        [SerializeField]
        private GameObject MainMenuScenePrefab, GameScenePrefab;

        [SerializeField]
        private LoadingScreen loadingScreen;

        public WorldFileIO MapFileIO { get; private set; }
        public GameObjectFactory Factory { get; private set; }
        public WorldGenerator WorldGenerator { get; private set; }

        private GameObject currentScene;
        public GameWorld World { get; private set; }
        public PlayerController PlayerController { get; private set; }

        public string MapFilePath { get; set; }

        public bool LoadFinished { get; private set; }
        public bool Paused { get; set; }

        public void Awake()
        {
            loadingScreen = FindObjectOfType<LoadingScreen>();
            Application.wantsToQuit += () =>
            {
                if (!Application.isEditor)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                return true;
            };
            StartCoroutine(StartLoading());
        }

        public void GenerateMap(GenerationSettings settings)
        {
            LoadGameSceneAsync(() =>
            {
                MapFilePath = "";
                WorldGenerator.Generate(settings, World);
            });
        }

        public void LoadMap(string filePath)
        {
            LoadGameSceneAsync(() =>
            {
                MapFilePath = filePath;
                var instanceData = MapFileIO.Load(filePath);
                World.Load(instanceData, Factory);
                instanceData = null;
            });
        }

        public void SaveMap(string fileName)
        {
            var instanceData = World.Save();
            var path = MapFileIO.GetSavePath(fileName);
            MapFileIO.Save(path, instanceData);
        }

        public void LoadMenus()
        {
            SwitchScenesAsync(MainMenuScenePrefab, oldSceneUnload: GameSceneUnload());
        }

        private void LoadGameSceneAsync(Action onComplete)
        {
            SwitchScenesAsync(GameScenePrefab, newSceneLoad: GameSceneLoad(onComplete));
        }

        private IEnumerator StartLoading()
        {
            loadingScreen.Open();
            yield return null;

            Factory = new GameObjectFactory(this);
            MapFileIO = new();
            WorldGenerator = new(Factory);
            loadingScreen.SetProgress(0.9f);
            yield return null;

            currentScene = Instantiate(MainMenuScenePrefab);
            loadingScreen.SetProgress(1f);
            yield return null;

            loadingScreen.Close();
        }

        private void SwitchScenesAsync(GameObject newScenePrefab,
            IEnumerator<float> oldSceneUnload = null,
            IEnumerator<float> newSceneLoad = null)
        {
            StartCoroutine(SceneSwitch(newScenePrefab, oldSceneUnload, newSceneLoad));
        }

        private IEnumerator SceneSwitch(GameObject newScenePrefab,
            IEnumerator<float> oldSceneUnload = null,
            IEnumerator<float> newSceneLoad = null)
        {
            LoadFinished = false;
            loadingScreen.Open();
            currentScene.SetActive(false);
            yield return null;

            var unload = SceneSwitch_UnloadOld(oldSceneUnload);
            while (unload.MoveNext())
            {
                yield return unload.Current;
            }

            var cleaning = SceneSwitch_CleanMemory();
            while (cleaning.MoveNext())
            {
                yield return cleaning.Current;
            }

            var load = SceneSwitch_LoadNew(newScenePrefab, newSceneLoad);
            while (load.MoveNext())
            {
                yield return load.Current;
            }

            loadingScreen.Close();
            currentScene.SetActive(true);
            yield return null;

            LoadFinished = true;
            yield return null;
        }

        private IEnumerator SceneSwitch_UnloadOld(IEnumerator<float> oldSceneUnload = null)
        {
            loadingScreen.SetProgress(0);
            loadingScreen.Text = "Выгрузка...";
            yield return null;

            var unload = ProgressableAction(oldSceneUnload);
            while (unload.MoveNext())
            {
                yield return null;
            }

            Destroy(currentScene);
            loadingScreen.SetProgress(1);
            yield return null;
        }

        private IEnumerator SceneSwitch_CleanMemory()
        {
            loadingScreen.Text = "Очистка...";
            for (float i = 0; i <= 1; i += 0.1f)
            {
                GarbageCollector.CollectIncremental(10 * 1000 * 1000UL);
                loadingScreen.SetProgress(i);
                yield return null;
            }
        }

        private IEnumerator SceneSwitch_LoadNew(GameObject newScenePrefab, IEnumerator<float> newSceneLoad = null)
        {
            loadingScreen.Text = "Загрузка...";
            loadingScreen.SetProgress(0);
            currentScene = Instantiate(newScenePrefab);
            currentScene.SetActive(false);
            yield return null;

            if (newSceneLoad != null)
            {
                var load = ProgressableAction(newSceneLoad);
                while (load.MoveNext())
                {
                    yield return null;
                }
            }
            loadingScreen.SetProgress(1);
            yield return null;
        }

        private IEnumerator ProgressableAction(IEnumerator<float> action)
        {
            if (action != null)
            {
                while (action.MoveNext())
                {
                    var progress = action.Current;
                    progress = Math.Clamp(progress, 0.01f, 0.99f);
                    loadingScreen.SetProgress(progress);
                    yield return null;
                }
            }
        }

        private IEnumerator<float> GameSceneUnload()
        {
            Paused = true;
            PlayerController.Stop();
            yield return 0.1f;

            Factory.DestroyContext();
            yield return 0.9f;

            World = null;
            PlayerController = null;
            yield return 1f;
        }

        private IEnumerator<float> GameSceneLoad(Action onComplete)
        {
            Paused = true;
            Factory.CreateContext();
            World = new GameWorld();
            yield return 0.05f;

            onComplete();
            yield return 0.4f;

            World.Init();
            yield return 0.95f;

            PlayerController = new(World.Player);
            PlayerController.WorkStart += () => Paused = false;
            PlayerController.ProgramStopped += () => Paused = true;
            yield return 1;
        }
    }
}
