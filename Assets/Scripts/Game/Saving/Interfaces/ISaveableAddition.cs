using System.Collections.Generic;

namespace Uninstructed.Game.Saving.Interfaces
{
    public interface ISaveableAddition
    {
        public void Save(IDictionary<string, object> data);
        public void Load(IDictionary<string, object> data, GameObjectFactory factory);
    }
}
