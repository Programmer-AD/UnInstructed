using System;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.IO;
using UnityEngine;
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
            var sceneLoading = SceneManager.LoadSceneAsync("GameScene");
            sceneLoading.completed += (_) =>
            {
                GameWorld = Instantiate(worldPrefab);
                onComplete();
                GameWorld.Init();
            };
        }
    }
}
