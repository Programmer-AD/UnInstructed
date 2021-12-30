using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.Game
{
    public class GameDirector : MonoBehaviour
    {
        public WorldFileIO MapFileIO { get; private set; }
        public GameObjectFactory GameObjectFactory { get; private set; }
        public WorldGenerator WorldGenerator { get; private set; }

        public string MapFileName { get; set; }
        private GameWorld GameWorld { get; set; }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
            MapFileIO = new WorldFileIO();
            GameObjectFactory = GetComponent<GameObjectFactory>();
            WorldGenerator=new WorldGenerator(GameObjectFactory);
        }

        public void GenerateMap(GenerationSettings settings)
        {
            SceneManager.LoadScene("GameScene");
            GameWorld = FindObjectOfType<GameWorld>();

            WorldGenerator.Generate(settings, GameWorld);
        }

        public void LoadMap(string mapFileName)
        {
            MapFileName = mapFileName;
            var instanceData = MapFileIO.Load(mapFileName);

            SceneManager.LoadScene("GameScene");
            GameWorld = FindObjectOfType<GameWorld>();
            GameWorld.Load(instanceData, GameObjectFactory);
        }

        public void SaveMap(string saveName)
        {
            var instanceData = GameWorld.Save();
            MapFileIO.Save(saveName, instanceData);
        }

        public void LoadMenus()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
