using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Uninstructed.Game.Main
{
    public class Item : MonoBehaviour
    {
        [SerializeField, Min(1)]
        private int maxCount;

        [SerializeField]
        private string itemName;

        public int MaxCount { get => maxCount; }
        public string ItemName { get => itemName; }
        public string ShowName { get; set; }

        private int count;
        public int Count
        {
            get => count;
            set
            {
                count = Math.Clamp(value, 0, maxCount);
            }
        }

        public void Start()
        {
            ShowName = ItemName;
        }

        public void Reset()
        {
            maxCount = 1;
            itemName = "Item Name";
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

        public bool CanUseSignle => UsedSingle != null;
        public event Action<Entity, Item> UsedSingle;
        public void Use(Entity user)
        {
            UsedSingle?.Invoke(user, this);
        }

        public bool CanUseOnBlock => UsedOnBlock != null;
        public event Action<Entity, Item, Block> UsedOnBlock;
        public void UseOnBlock(Entity user, Block block)
        {
            UsedOnBlock?.Invoke(user, this, block);
        }
    }
}
