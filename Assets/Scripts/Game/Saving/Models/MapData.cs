using System;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class MapData
    {
        public int Width, Height;
        public BlockData[] Blocks;
    }
}
