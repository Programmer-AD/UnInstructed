using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
