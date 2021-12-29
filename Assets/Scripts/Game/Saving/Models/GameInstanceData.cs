using System;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameInstanceData : GameInstancePreviewData
    {
        public BlockData[,] Map;
        public EntityData[] Entities;
        public ItemData[] DroppedItems;
    }
}
