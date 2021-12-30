﻿using System;

namespace Uninstructed.Game.Saving.Models
{
    [Serializable]
    public class GameWorldPreviewData
    {
        public string MapName;
        public DateTime SaveDate;

        [NonSerialized]
        public string FileName;
    }
}