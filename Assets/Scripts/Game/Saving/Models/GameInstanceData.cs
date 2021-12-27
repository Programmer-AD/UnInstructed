using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameInstanceData:GameInstancePreviewData
    {
        public BlockData[,] Map;
        public EntityData[] Entities;
        public ItemData[] DroppedItems;
    }
}
