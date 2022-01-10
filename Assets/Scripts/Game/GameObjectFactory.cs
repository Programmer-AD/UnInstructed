using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Models;
using UnityEditor;
using UnityEngine;

namespace Uninstructed.Game
{
    public class GameObjectFactory
    {
        private readonly Dictionary<ItemType, Item> items;
        private readonly Dictionary<EntityType, Entity> entities;
        private readonly Dictionary<BlockType, Block> blocks;

        private readonly Item unknownItem;
        private readonly Entity unknownEntity;
        private readonly Block unknownBlock;

        public GameDirector Director { get; }

        private Transform containerTransform;

        public GameObjectFactory(GameDirector director)
        {
            Director = director;

            blocks = LoadStructured<BlockType, Block, BlockData>("Block");
            entities = LoadStructured<EntityType, Entity, EntityData>("Entity");
            items = LoadStructured<ItemType, Item, ItemData>("Item");

            blocks.TryGetValue(BlockType.Unknown, out unknownBlock);
            entities.TryGetValue(EntityType.Unknown, out unknownEntity);
            items.TryGetValue(ItemType.Unknown, out unknownItem);
        }

        public void CreateContext()
        {
            var container = new GameObject("GameContentContainer");
            containerTransform = container.transform;
        }

        public void DestroyContext()
        {
            UnityEngine.Object.Destroy(containerTransform.gameObject);
        }

        public Entity Load(EntityData data)
        {
            return Load(data, unknownEntity, entities);
        }

        public Block Load(BlockData data)
        {
            return Load(data, unknownBlock, blocks);
        }

        public Item Load(ItemData data)
        {
            return Load(data, unknownItem, items);
        }

        public Entity Create(EntityType type)
        {
            return Create<EntityType, EntityData, Entity>(type, unknownEntity, entities);
        }

        public Block Create(BlockType type)
        {
            return Create<BlockType, BlockData, Block>(type, unknownBlock, blocks);
        }

        public Item Create(ItemType type)
        {
            return Create<ItemType, ItemData, Item>(type, unknownItem, items);
        }

        private TObject Load<TEnum, TObject, TMemento>(TMemento memento, TObject unknown, Dictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var result = CreateEmpty<TEnum, TMemento, TObject>(memento.Type, unknown, prefabs);
            result.Load(memento, this);
            return result;
        }

        private TObject Create<TEnum, TMemento, TObject>(TEnum type, TObject unknown, Dictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var result = CreateEmpty<TEnum, TMemento, TObject>(type, unknown, prefabs);
            result.InitDefault(this);
            return result;
}

        private TObject CreateEmpty<TEnum, TMemento, TObject>(TEnum type, TObject unknown, Dictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var prefab = GetPrefab(type, unknown, prefabs);
            var result = UnityEngine.Object.Instantiate(prefab);
            result.transform.SetParent(containerTransform);
            result.Director = Director;
            return result;
        }

        private TObject GetPrefab<TEnum, TObject>(TEnum type, TObject unknown, Dictionary<TEnum, TObject> prefabs)
            where TObject : class
            where TEnum : Enum
        {
            if (prefabs.ContainsKey(type))
            {
                return prefabs[type];
            }

            return unknown;
        }

        private Dictionary<TEnum, TObject> LoadStructured<TEnum, TObject, TMemento>(string path)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var objects = Resources.LoadAll<TObject>(path);
            var result = objects.ToDictionary(x => x.Type, x => x);
            return result;
        }
    }
}
