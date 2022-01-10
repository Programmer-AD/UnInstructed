using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game
{
    public class GameWorld : ISaveable<GameWorldData>
    {
        public List<Entity> Entities { get; set; }
        public List<Item> DroppedItems { get; set; }
        public Map Map { get; set; }
        public string MapName { get; set; }

        public Entity Player { get; private set; }

        public void Init()
        {
            Map.Init();
            Player = Entities.First(x => x.Type == EntityType.Player);
        }

        public void Load(GameWorldData memento, GameObjectFactory factory)
        {
            MapName = memento.MapName;
            Map = new Map();
            Map.Load(memento.Map, factory);
            DroppedItems = memento.DroppedItems.Select(x => factory.Load(x)).ToList();
            Entities = memento.Entities.Select(x => factory.Load(x)).ToList();
        }

        public GameWorldData Save()
        {
            Optimize();
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

        public void Optimize()
        {
            DroppedItems = DroppedItems.Where(x => x != null).ToList();
            Entities = Entities.Where(x => x != null && !x.Dead).ToList();
            Map.Optimize();
        }
    }
}
