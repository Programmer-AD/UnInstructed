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
        public int MaxCount => maxCount;

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

        private Entity dropper;

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

        public void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.TryGetComponent(out Entity entity))
            {
                if (entity != dropper)
                {
                    entity.Inventory.Add(this);
                    Optimize();
                }
            }
            else if (other.gameObject.TryGetComponent(out Item item))
            {
                if (item.Type == Type && Count + item.Count <= maxCount)
                {
                    item.Count += Count;
                    Count = 0;
                }
            }
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Entity entity))
            {
                if (entity == dropper)
                {
                    dropper = null;
                }
            }
        }

        public void Optimize()
        {
            if (Count == 0)
            {
                OnScene = false;
                Destroy(this);
            }
        }

        public void Drop(Entity dropper, int? count)
        {
            var dropped = this;
            if (count.HasValue && count != Count)
            {
                if (count > Count)
                {
                    return;
                }
                dropped = Instantiate(this);
                dropped.count = count.Value;
            }
            dropped.dropper = dropper;
            dropped.transform.position = dropper.transform.position;
            dropped.OnScene = true;
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

        protected override void InitDefaultSub(GameObjectFactory factory) { }
    }
}
