using System;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class EntityData : GameObjectData<EntityType>
    {
        public float X, Y, Rotation, Health;
        public int SelectedInventorySlot;
        public ItemData[] Inventory;
    }
}
