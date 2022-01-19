using System.Collections.Generic;

namespace Uninstructed.Game.Saving.Interfaces
{
    public interface ISaveableAddition
    {
        public void Save(Dictionary<string, object> data);
        public void Load(Dictionary<string, object> data, GameObjectFactory factory);
    }
}
