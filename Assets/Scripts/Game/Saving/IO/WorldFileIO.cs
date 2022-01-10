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
            var path = Path.Combine(SaveLocation, fileName);
            return path;
        }

        public void Save(string filePath, GameWorldData instanceData)
        {
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var previewData = instanceData.CopyPreview();
            formatter.Serialize(stream, previewData);
            formatter.Serialize(stream, instanceData);
        }

        public GameWorldPreviewData LoadPreview(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var result = formatter.Deserialize(stream);
            return (GameWorldPreviewData)result;
        }
        public GameWorldData Load(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            _ = formatter.Deserialize(stream);
            var result = formatter.Deserialize(stream);
            return (GameWorldData)result;
        }

        public GameWorldPreviewData[] GetPreviewList()
        {
            var files = Directory.GetFiles(SaveLocation);

            var previews = new List<GameWorldPreviewData>();
            foreach (var file in files)
            {
                if (file.EndsWith(FileFormat))
                {
                    try
                    {
                        var preview = LoadPreview(file);
                        preview.FileName = file;
                        previews.Add(preview);
                    }
                    catch (Exception) { }
                }
            }
            var result = previews.OrderByDescending(x => x.SaveDate).ToArray();

            return result;
        }
    }
}
