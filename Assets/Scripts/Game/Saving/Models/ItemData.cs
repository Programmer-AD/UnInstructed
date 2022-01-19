using System;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class ItemData : GameObjectData<ItemType>
    {
        public float? X, Y;
        public int Count;
    }
}
