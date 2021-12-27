using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Saving
{
    [Serializable]
    public class EntityData : GameObjectData<EntityType>
    {
        public float X, Y, Rotation, Health;
        public bool Dead;
        public int SelectedInventorySlot;
    }
}
