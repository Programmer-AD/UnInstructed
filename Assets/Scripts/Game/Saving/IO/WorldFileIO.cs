using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Saving.IO
{
    public class WorldFileIO
    {
        public static readonly string SaveLocation = Application.dataPath + "/PlayerSaves/";
        public const string FileFormat = ".uninwrld";

        private readonly BinaryFormatter formatter = new();

        public WorldFileIO()
        {
            if (!Directory.Exists(SaveLocation))
            {
                Directory.CreateDirectory(SaveLocation);
            }
        }

        public string GetSavePath(string fileName)
        {
            if (!fileName.EndsWith(FileFormat))
            {
                fileName += FileFormat;
            }
            string path = Path.Combine(SaveLocation, fileName);
            return path;
        }

        public void Save(string filePath, GameWorldData instanceData)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var previewData = (GameWorldPreviewData)instanceData;
            formatter.Serialize(stream, previewData);
            formatter.Serialize(stream, instanceData);
            stream.Flush();
        }

        public GameWorldPreviewData LoadPreview(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            object result = formatter.Deserialize(stream);
            return (GameWorldPreviewData)result;
        }
        public GameWorldData Load(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            object preview = formatter.Deserialize(stream);
            object result = formatter.Deserialize(stream);
            return (GameWorldData)result;
        }

        public IList<GameWorldPreviewData> GetPreviewList()
        {
            string[] files = Directory.GetFiles(SaveLocation);
            var previewFiles = files.Where(x => x.EndsWith(FileFormat));
            var previews = previewFiles.Select(file =>
            {
                try
                {
                    var preview = LoadPreview(file);
                    preview.FileName = file;
                    return preview;
                }
                catch (Exception)
                {
                    return null;
                }
            }).Where(x => x != null).ToList();
            return previews;
        }
    }
}
