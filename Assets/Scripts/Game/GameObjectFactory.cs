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

        public void Reset()
        {
            blockPrefabsPath = "Assets/Prefabs/Game/Block";
            entityPrefabsPath = "Assets/Prefabs/Game/Entity";
            itemPrefabsPath = "Assets/Prefabs/Game/Item";
        }

        public void Start()
        {
            items = GetStructured<ItemType, Item, ItemData>(itemPrefabsPath);
            entities = GetStructured<EntityType, Entity, EntityData>(entityPrefabsPath);
            blocks = GetStructured<BlockType, Block, BlockData>(blockPrefabsPath);
        }

        public Entity Load(EntityData data)
            => Load(data, entities);
        public Block Load(BlockData data)
            => Load(data, blocks);
        public Item Load(ItemData data)
            => Load(data, items);

        public Entity Create(EntityType type)
            => Create<EntityType, EntityData, Entity>(type, entities);
        public Block Create(BlockType type)
            => Create<BlockType, BlockData, Block>(type, blocks);
        public Item Create(ItemType type)
            => Create<ItemType, ItemData, Item>(type, items);

        private TObject Load<TEnum, TObject, TMemento>(TMemento memento, IDictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var result = prefabs[memento.Type];
            result.Load(memento, this);
            return result;
        }

        private TObject Create<TEnum, TMemento, TObject>(TEnum type, IDictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>, new()
            where TEnum : Enum
        {
            var prefab = prefabs[type];
            var result = Instantiate(prefab);
            result.InitDefault(this);
            return result;
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
