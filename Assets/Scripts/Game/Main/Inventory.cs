using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Main
{
    public class Inventory : IEnumerable<Item>
    {
        private readonly Slot[] slots;

        public Inventory(int maxSize)
        {
            slots = new Slot[maxSize];
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i] = new Slot(i);
            }
        }

        public int Size => slots.Length;
        public Item this[int index]
        {
            get => slots[index].Item;
            set => slots[index].Item = value;
        }

        public void Add(Item item)
        {
            if (item.Count > 0)
            {
                var canAddTo = slots.Where(x => !x.Empty && !x.Full && x.Item.Type == item.Type)
                    .OrderBy(x => x.Number).GetEnumerator();
                while (item.Count > 0 && canAddTo.MoveNext())
                {
                    var slot = canAddTo.Current;
                    var addCount = Math.Min(slot.CanAdd, item.Count);

                    slot.Item.Count += addCount;
                    item.Count -= addCount;
                }

                if (item.Count > 0)
                {
                    var emptySlot = slots.Where(x => x.Item == null).FirstOrDefault();
                    if (emptySlot != null)
                    {
                        emptySlot.Item = UnityEngine.Object.Instantiate(item);
                        emptySlot.Item.OnScene = false;
                        item.Count = 0;
                    }
                }

                item.Optimize();
            }
        }

        public int TotalCount(ItemType type)
        {
            return slots.Where(x => !x.Empty && x.Item.Type == type)
                .Select(x => x.Item.Count).Sum();
        }

        public int Remove(ItemType type, int count)
        {
            if (count > 0)
            {
                var toRemove = count;
                var canRemoveFrom = slots.Where(x => !x.Empty && x.Item.Type == type)
                    .OrderByDescending(x => x.Number).GetEnumerator();

                while (toRemove > 0 && canRemoveFrom.MoveNext())
                {
                    var slot = canRemoveFrom.Current;
                    var removeCount = Math.Min(toRemove, slot.Item.Count);
                    slot.Item.Count -= removeCount;
                    toRemove -= removeCount;
                }
                Recheck();
                return count - toRemove;
            }
            return 0;
        }

        public void Recheck()
        {
            foreach (var slot in slots)
            {
                slot.Normalize();
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            foreach (var slot in slots)
            {
                yield return slot.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Slot
        {
            public Item Item;

            public Slot(int number)
            {
                Number = number;
            }

            public int Number { get; }

            public bool Empty => Item == null;
            public bool Full => !Empty && Item.Count == Item.MaxCount;
            public int CanAdd => Empty ? 0 : Item.MaxCount - Item.Count;

            public void Normalize()
            {
                if (!Empty && Item.Count == 0)
                {
                    Item.Optimize();
                    Item = null;
                }
            }
        }
    }
}
