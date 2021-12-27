using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving;
using UnityEngine;

namespace Uninstructed.Game
{
    public class GameObjectFactory : MonoBehaviour
    {
        [SerializeField]
        private string itemPrefabPath, entityPrefabPath, blockPrefabPath;

        private IDictionary<ItemType, Item> items;
        private IDictionary<EntityType, Entity> entities;
        private IDictionary<BlockType, Block> blocks;

        public void Reset()
        {
            itemPrefabPath = "Assets/Prefabs/";
            entityPrefabPath = "Assets/Prefabs/";
            blockPrefabPath = "Assets/Prefabs/";
        }

        public void Start()
        {
            items = GetStructured<ItemType, Item, ItemData>(itemPrefabPath);
            entities = GetStructured<EntityType, Entity, EntityData>(entityPrefabPath);
            blocks = GetStructured<BlockType, Block, BlockData>(blockPrefabPath);
        }

        public Entity Create(EntityData data)
        {
            var result = Create(data, entities);

            int position = 0;
            foreach(var itemData in data.Inventory)
            {
                if (itemData != null)
                {
                    result.Inventory[position++] = Create(itemData);
                }
            }

            return result;
        }
        public Block Create(BlockData data) 
            => Create(data, blocks);
        public Item Create(ItemData data)
            => Create(data, items);

        private TObject Create<TEnum, TObject, TMemento>(TMemento memento, IDictionary<TEnum, TObject> prefabs)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>
            where TEnum : Enum
        {
            var prefab = prefabs[memento.Type];
            var result = Instantiate(prefab);
            result.Load(memento);
            return result;
        }

        private IDictionary<TEnum, TObject> GetStructured<TEnum, TObject, TMemento>(string path)
            where TObject : GameObjectBase<TEnum, TMemento>
            where TMemento : GameObjectData<TEnum>
            where TEnum : Enum
        {
            var objects = Resources.LoadAll<TObject>(path);
            var result = objects.ToDictionary(x => x.Type, x => x);
            return result;
        }
    }
}
