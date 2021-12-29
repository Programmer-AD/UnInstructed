using System;
using System.Collections.Generic;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameObjectData<TEnum>
        where TEnum : Enum
    {
        public TEnum Type;
        public string ShowName;

        public IDictionary<string, object> Additionals = new Dictionary<string, object>();
    }
}
