using System;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class BlockData : GameObjectData<BlockType>
    {
        public float Durability;
        public bool CanGoThrough;
    }
}
