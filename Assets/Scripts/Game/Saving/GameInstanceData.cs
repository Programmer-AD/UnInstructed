using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;

namespace Uninstructed.Game.Saving
{
    [Serializable]
    public class GameInstanceData
    {
        public BlockData[,] Map;
        public EntityData[] Entities { get; set; }
        public ItemData[] DroppedItems { get; set; }
    }
}
