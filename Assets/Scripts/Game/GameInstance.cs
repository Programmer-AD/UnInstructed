using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Uninstructed.Game
{
    public class GameInstance : MonoBehaviour, ISaveable<GameInstanceData>
    {
        public Tilemap Tilemap;

        public Entity Player { get; private set; }
        public IList<Entity> Entities { get; private set; }
        public IList<Item> DroppedItems { get; private set; }
        public string MapName { get; set; }

        public int MapWidth => Tilemap.cellBounds.size.x;
        public int MapHeight => Tilemap.cellBounds.size.y;

        public void Load(GameInstanceData memento, GameObjectFactory factory)
        {
            MapName = memento.MapName;
            Entities = memento.Entities.Select(x => factory.Load(x)).ToList();
            DroppedItems = memento.DroppedItems.Select(x => factory.Load(x)).ToList();
            LoadMap(memento.Map, factory);

            Player = Entities.First(x => x.Type == EntityType.Player);
        }

        private void LoadMap(BlockData[,] map, GameObjectFactory factory)
        {
            int mapWidth = map.GetLength(0);
            int mapHeight = map.GetLength(1);
            Tilemap.ClearAllTiles();

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var block = factory.Load(map[x, y]);
                    var tile = ScriptableObject.CreateInstance<Tile>();
                    tile.gameObject = block.gameObject;
                    tile.sprite = block.GetComponent<SpriteRenderer>().sprite;
                    tile.colliderType = block.CanGoThrough ? Tile.ColliderType.None : Tile.ColliderType.Sprite;
                    Tilemap.SetTile(new Vector3Int(x, y), tile);
                }
            }
        }

        public GameInstanceData Save()
        {
            var memento = new GameInstanceData
            {
                MapName = MapName,
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
