using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game
{
    public class GameObjectFactory : MonoBehaviour
    {
        [SerializeField]
        private string blockPrefabsPath, entityPrefabsPath, itemPrefabsPath;

        private IDictionary<ItemType, Item> items;
        private IDictionary<EntityType, Entity> entities;
        private IDictionary<BlockType, Block> blocks;

        private Item unknownItem;
        private Entity unknownEntity;
        private Block unknownBlock;

        public void Reset()
        {
            blockPrefabsPath = "Block";
            entityPrefabsPath = "Entity";
            itemPrefabsPath = "Item";
        }

        public void Start()
        {
            items = GetStructured<ItemType, Item, ItemData>(itemPrefabsPath);
            entities = GetStructured<EntityType, Entity, EntityData>(entityPrefabsPath);
            blocks = GetStructured<BlockType, Block, BlockData>(blockPrefabsPath);

            items.TryGetValue(ItemType.Unknown, out unknownItem);
            entities.TryGetValue(EntityType.Unknown, out unknownEntity);
            blocks.TryGetValue(BlockType.Unknown, out unknownBlock);
        }

        public Entity Load(EntityData data)
            => Load(data, unknownEntity, entities);
        public Block Load(BlockData data)
            => Load(data, unknownBlock, blocks);
        public Item Load(ItemData data)
            => Load(data, unknownItem, items);

        public Entity Create(EntityType type)
            => Create<EntityType, EntityData, Entity>(type, unknownEntity, entities);
        public Block Create(BlockType type)
            => Create<BlockType, BlockData, Block>(type, unknownBlock, blocks);
        public Item Create(ItemType type)
            => Create<ItemType, ItemData, Item>(type, unknownItem, items);

        private TObject Load<TEnum, TObject, TMemento>(TMemento memento, TObject unknown, IDictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var prefab = GetPrefab(memento.Type, unknown, prefabs);
            var result = Instantiate(prefab);
            result.Load(memento, this);
            return result;
        }

        private TObject Create<TEnum, TMemento, TObject>(TEnum type, TObject unknown, IDictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var prefab = GetPrefab(type, unknown, prefabs);
            var result = Instantiate(prefab);
            result.InitDefault(this);
            return result;
        }

        private TObject GetPrefab<TEnum, TObject>(TEnum type, TObject unknown, IDictionary<TEnum, TObject> prefabs)
            where TObject : class
            where TEnum : Enum
        {
            if (prefabs.ContainsKey(type))
            {
                return prefabs[type];
            }

            return unknown;
        }

        private IDictionary<TEnum, TObject> GetStructured<TEnum, TObject, TMemento>(string path)
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
