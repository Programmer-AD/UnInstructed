using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstructed.Game.Saving.Interfaces
{
    public interface ISaveable<T> where T : class
    {
        T Save();
        void Load(T memento, GameObjectFactory factory);
    }
}
