using System;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Item : GameObjectBase<ItemType, ItemData>
    {
        [SerializeField, Min(1)]
        private int maxCount;
        public int MaxCount { get => maxCount; }


        private int count;
        public int Count
        {
            get => count;
            set
            {
                count = Math.Clamp(value, 0, maxCount);
                Optimize();
            }

        }

        public override void Reset()
        {
            base.Reset();

            maxCount = 1;
        }

        public bool OnScene
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void Optimize()
        {
            if (Count == 0)
            {
                Destroy(this);
            }
        }


        public event Action<Entity, Item> UsedSingle;
        public void Use(Entity user)
        {
            UsedSingle?.Invoke(user, this);
        }

        public event Action<Entity, Item, Block> UsedOnBlock;
        public void UseOnBlock(Entity user, Block block)
        {
            UsedOnBlock?.Invoke(user, this, block);
        }

        protected override void LoadSub(ItemData memento, GameObjectFactory factory)
        {
            Count = memento.Count;
            OnScene = memento.X != null;
            if (OnScene)
            {
                transform.position = new Vector3(memento.X.Value, memento.Y.Value);
            }
        }

        protected override void SaveSub(ItemData memento)
        {
            memento.Count = Count;
            if (OnScene)
            {
                memento.X = transform.position.x;
                memento.Y = transform.position.y;
            }
        }
    }
}
