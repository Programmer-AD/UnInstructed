﻿using System;
using System.Collections;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Player;
using Uninstructed.Game.Saving.IO;
using Uninstructed.UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.Game
{
    public class GameDirector : MonoBehaviour
    {
        public WorldFileIO MapFileIO { get; private set; }
        public GameObjectFactory GameObjectFactory { get; private set; }
        public WorldGenerator WorldGenerator { get; private set; }
        public GameWorld World { get; private set; }
        public PlayerController PlayerController { get; private set; }

        public string MapFilePath { get; set; }

        public bool LoadFinished { get; private set; }
        public bool Paused { get; set; }

        public void Start()
        {
            var existingDirector = FindObjectOfType<GameDirector>();
            if (existingDirector != this && existingDirector != null)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);

            GameObjectFactory = new GameObjectFactory(this);
            MapFileIO = new();
            WorldGenerator = new(GameObjectFactory);
        }

        public void GenerateMap(GenerationSettings settings)
        {
            LoadGameSceneAsync(() =>
            {
                World.MapName = settings.MapName;
                WorldGenerator.Generate(settings, World);
            });
        }

        public void LoadMap(string filePath)
        {
            LoadGameSceneAsync(() =>
            {
                MapFilePath = filePath;
                var instanceData = MapFileIO.Load(filePath);
                World.Load(instanceData, GameObjectFactory);
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

            var loadAsync = SceneManager.LoadSceneAsync("MainMenu");
            loadAsync.priority = int.MaxValue;
            loadAsync.completed += _ =>
            {
                GC.Collect();
            };
        }

        private void LoadGameSceneAsync(Action onComplete)
        {
            StartCoroutine(GameSceneLoading(onComplete));
        }

        private IEnumerator GameSceneLoading(Action onComplete)
        {
            LoadFinished = false;

            var loadingScreen = FindObjectOfType<LoadingScreen>(true);
            loadingScreen.Open();
            yield return null;

            var sceneLoading = SceneManager.LoadSceneAsync("GameScene");
            sceneLoading.allowSceneActivation = true;
            sceneLoading.completed += _ =>
            {
                StartCoroutine(GameSceneBuilding(onComplete));
            };

            var progress = sceneLoading.progress;
            while (!sceneLoading.isDone)
            {
                var newProgress = sceneLoading.progress;
                if (progress != newProgress)
                {
                    progress = newProgress;
                    loadingScreen.SetProgress(progress);
                }
                yield return null;
            }
        }

        private IEnumerator GameSceneBuilding(Action onComplete)
        {
            var buildingScreen = FindObjectOfType<LoadingScreen>(true);
            buildingScreen.Open();
            yield return null;

            Paused = true;
            World = new GameWorld();
            buildingScreen.SetProgress(0.05f);
            yield return null;

            onComplete();
            buildingScreen.SetProgress(0.4f);
            yield return null;

            World.Init();
            buildingScreen.SetProgress(0.99f);
            yield return null;

            PlayerController = new(World.Player);
            PlayerController.WorkStart += () => Paused = false;
            PlayerController.ProgramStopped += () => Paused = true;
            buildingScreen.SetProgress(1f);
            yield return null;


            LoadFinished = true;
            buildingScreen.Close();
            yield return null;
        }
    }
}
