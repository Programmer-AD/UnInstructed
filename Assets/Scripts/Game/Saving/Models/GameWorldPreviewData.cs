using System;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameWorldPreviewData
    {
        public string MapName;
        public DateTime SaveDate;

        [NonSerialized]
        public string FileName;

        public GameWorldPreviewData CopyPreview()
        {
            var preview = new GameWorldPreviewData
            {
                MapName = MapName,
                SaveDate = SaveDate,
                FileName = FileName
            };
            return preview;
        }
    }
}
