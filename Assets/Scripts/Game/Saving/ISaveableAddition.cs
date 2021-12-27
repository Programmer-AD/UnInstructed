using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstructed.Game.Saving
{
    public interface ISaveableAddition
    {
        void Save(IDictionary<string, object> container);
        void Load(IDictionary<string, object> container);
    }
}
