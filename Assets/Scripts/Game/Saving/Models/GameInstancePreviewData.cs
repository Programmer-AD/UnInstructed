using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameInstancePreviewData
    {
        public string MapName;
        public DateTime SaveDate;

        [NonSerialized]
        public string FileName;
    }
}
