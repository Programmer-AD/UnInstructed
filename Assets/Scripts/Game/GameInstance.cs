using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Uninstructed.Game
{
    public class GameInstance : MonoBehaviour, ISaveable<GameInstanceData>
    {
        private GameObjectFactory factory;

        public Tilemap Tilemap;

        public IList<Entity> Entities { get; set; }
        public IList<Item> DroppedItems { get; set; }
        public string MapName { get; set; }

        public int MapWidth => Tilemap.cellBounds.size.x;
        public int MapHeight => Tilemap.cellBounds.size.y;

        public void Start()
        {
            factory = GetComponent<GameObjectFactory>();
        }

        public void Load(GameInstanceData memento)
        {
            MapName = memento.MapName;
            Entities = memento.Entities.Select(x => factory.Create(x)).ToList();
            DroppedItems = memento.DroppedItems.Select(x => factory.Create(x)).ToList();
            LoadMap(memento.Map);
        }

        private void LoadMap(BlockData[,] map)
        {
            var mapWidth = map.GetLength(0);
            var mapHeight = map.GetLength(1);
            Tilemap.ClearAllTiles();

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var block = factory.Create(map[x, y]);
                    var tile = new Tile()
                    {
                        gameObject = block.gameObject,
                        sprite = block.GetComponent<SpriteRenderer>().sprite,
                        colliderType = Tile.ColliderType.Sprite,
                    };
                    Tilemap.SetTile(new Vector3Int(x,y), tile);
                }
            }
        }

        public GameInstanceData Save()
        {
            var memento = new GameInstanceData
            {
                MapName=MapName,
                SaveDate = DateTime.Now,
                DroppedItems = DroppedItems.Select(x => x.Save()).ToArray(),
                Entities = Entities.Select(x => x.Save()).ToArray(),  
            };
            SaveMap(memento);

            return memento;
        }
        private void SaveMap(GameInstanceData memento)
        {
            memento.Map = new BlockData[MapWidth, MapHeight];
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    var tile = (Tile)Tilemap.GetTile(new Vector3Int(x, y));
                    var block = tile.gameObject.GetComponent<Block>();
                    memento.Map[x, y] = block.Save();
                }
            }
        }
    }
}
