using System;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Mapping;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;

namespace Uninstructed.Game
{
    public class GameWorld : ISaveable<GameWorldData>
    {
        private readonly LazyUpdateable<Entity[]> entitiesUpdateable;
        public Entity[] Entities => entitiesUpdateable;

        private readonly LazyUpdateable<Item[]> itemsUpdateable;
        public Item[] DroppedItems => itemsUpdateable;

        public Map Map { get; set; }
        public string MapName { get; set; }

        public Entity Player { get; private set; }

        public GameWorld()
        {
            entitiesUpdateable = new(_ => UnityEngine.Object.FindObjectsOfType<Entity>());
            itemsUpdateable = new(_ => UnityEngine.Object.FindObjectsOfType<Item>());
        }

        public void Init()
        {
            Map.Init();
            Player = Entities.First(x => x.Type == EntityType.Player);
            entitiesUpdateable.ForceUpdate();
            itemsUpdateable.ForceUpdate();
        }

        public void Load(GameWorldData memento, GameObjectFactory factory)
        {
            MapName = memento.MapName;
            Map = new Map();
            Map.Load(memento.Map, factory);

            foreach (var item in memento.DroppedItems)
            {
                factory.Load(item);
            }
            foreach (var entity in memento.Entities)
            {
                factory.Load(entity);
            }
        }

        public GameWorldData Save()
        {
            RefreshData();
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

        public void SetItemsNeedUpdate()
        {
            itemsUpdateable.SetNeedUpdate();
        }

        public void SetEntitiesNeedUpdate()
        {
            entitiesUpdateable.SetNeedUpdate();
        }

        public void RefreshData()
        {
            Map.Optimize();
            itemsUpdateable.ForceUpdate();
            entitiesUpdateable.ForceUpdate();
        }
    }
}
