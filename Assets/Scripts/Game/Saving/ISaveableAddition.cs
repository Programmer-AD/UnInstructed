using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstructed.Game.Saving
{
    public interface ISaveableAddition
    {
        public void Save(IDictionary<string, object> data);
        public void Load(IDictionary<string, object> data);
    }
}
