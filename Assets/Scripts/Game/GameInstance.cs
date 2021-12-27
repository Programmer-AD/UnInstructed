using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving;
using UnityEngine;

namespace Uninstructed.Game
{
    public class GameInstance : MonoBehaviour, ISaveable<GameInstanceData>
    {
        private GameObjectFactory factory;

        public Block[,] Map { get; set; }
        public IList<Entity> Entities { get; set; }
        public IList<Item> DroppedItems { get; set; }

        public int MapWidth => Map.GetLength(0);
        public int MapHeight => Map.GetLength(1);

        public void Start()
        {
            factory = GetComponent<GameObjectFactory>();
        }

        public void Load(GameInstanceData memento)
        {
            LoadMap(memento.Map);
            Entities = memento.Entities.Select(x => factory.Create(x)).ToList();
            DroppedItems = memento.DroppedItems.Select(x => factory.Create(x)).ToList();
        }

        private void LoadMap(BlockData[,] map)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            Map = new Block[mapWidth, mapHeight];

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Map[x, y] = factory.Create(map[x, y]);
                }
            }
        }

        public GameInstanceData Save()
        {
            var memento = new GameInstanceData();
            SaveMap(memento);
            memento.DroppedItems = DroppedItems.Select(x => x.Save()).ToArray();
            memento.Entities = Entities.Select(x => x.Save()).ToArray();
            return memento;
        }
        private void SaveMap(GameInstanceData memento)
        {
            memento.Map = new BlockData[MapWidth, MapHeight];
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    memento.Map[x, y] = Map[x, y].Save();
                }
            }
        }
    }
}
