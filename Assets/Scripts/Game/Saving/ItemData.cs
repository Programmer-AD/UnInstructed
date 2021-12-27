using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Saving
{
    [Serializable]
    public class ItemData:GameObjectData<ItemType>
    {
        public float? X, Y; 
        public int Count;
    }
}
