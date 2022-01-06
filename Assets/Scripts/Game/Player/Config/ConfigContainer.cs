using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstructed.Game.Player.Config
{
    internal class ConfigContainer
    {
        private readonly BitArray values;
        
        public ConfigContainer()
        {
            var size = Enum.GetValues(typeof(ConfigKey)).Length;
            values = new(size);
        }

        public bool this[ConfigKey key]
        {
            get => values[(int)key];
            set => values[(int)key] = value;
        }
    }
}
