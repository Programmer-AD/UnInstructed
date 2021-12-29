using Uninstructed.Game.Saving.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uninstructed.Game
{
    public class GameDirector : MonoBehaviour
    {
        public MapFileIO MapFileIO { get; private set; }
        public GameObjectFactory GameObjectFactory { get; private set; }

        public string MapFileName { get; set; }
        private GameInstance GameInstance { get; set; }


        public void Start()
        {
            DontDestroyOnLoad(gameObject);
            MapFileIO = new MapFileIO();
            GameObjectFactory = GetComponent<GameObjectFactory>();
        }

        public void LoadMap(string mapFileName)
        {
            MapFileName = mapFileName;
            var instanceData = MapFileIO.Load(mapFileName);

            SceneManager.LoadScene("GameScene");
            GameInstance = FindObjectOfType<GameInstance>();
            GameInstance.Load(instanceData, GameObjectFactory);
        }

        public void SaveMap(string saveName)
        {
            var instanceData = GameInstance.Save();
            MapFileIO.Save(saveName, instanceData);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
