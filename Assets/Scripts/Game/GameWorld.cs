using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Uninstructed.Game
{
    public class GameWorld : MonoBehaviour, ISaveable<GameWorldData>
    {
        [SerializeField]
        private GameObject mapContainer;

        public IList<Entity> Entities { get; set; }
        public IList<Item> DroppedItems { get; set; }
        public Map Map { get; set; }
        public string MapName { get; set; }

        public Entity Player { get; private set; }

        public void Reset()
        {
            mapContainer = null;
        }

        public void Init()
        {
            Map.InitPositions(mapContainer.transform);
            Player = Entities.First(x => x.Type == EntityType.Player);
        }

        public void Load(GameWorldData memento, GameObjectFactory factory)
        {
            MapName = memento.MapName;
            Entities = memento.Entities.Select(x => factory.Load(x)).ToList();
            DroppedItems = memento.DroppedItems.Select(x => factory.Load(x)).ToList();
            Map = new Map();
            Map.Load(memento.Map, factory);
        }

        public GameWorldData Save()
        {
            var memento = new GameWorldData
            {
                MapName = MapName,
                SaveDate = DateTime.Now,
                DroppedItems = DroppedItems.Select(x => x.Save()).ToArray(),
                Entities = Entities.Select(x => x.Save()).ToArray(),
                Map = Map.Save(),
            };

            return memento;
        }
    }
}
