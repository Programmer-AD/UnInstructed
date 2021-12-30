using System;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameWorldData : GameWorldPreviewData
    {
        public MapData Map;
        public EntityData[] Entities;
        public ItemData[] DroppedItems;
    }
}
