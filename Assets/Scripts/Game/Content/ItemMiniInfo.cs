using System;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Content
{
    [Serializable]
    public struct ItemMiniInfo
    {
        public ItemType ItemType;
        public int Count;

        public void Deconstruct(out ItemType itemType, out int count)
        {
            itemType = ItemType;
            count = Count;
        }
    }
}
