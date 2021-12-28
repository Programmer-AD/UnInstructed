using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Saving.IO
{
    public static class InstanceSaver
    {
        private static readonly BinaryFormatter formatter = new();
        public static readonly string SaveLocation = Application.dataPath + "/PlayerSaves";
        public static readonly string FileFormat = ".uninmap";

        public static void Save(string fileName, GameInstanceData instanceData)
        {
            if (!fileName.EndsWith(FileFormat))
            {
                fileName += FileFormat;
            }
            using var stream = new FileStream($"{SaveLocation}/{fileName}", FileMode.Create, FileAccess.Write);
            var previewData = (GameInstancePreviewData)instanceData;
            formatter.Serialize(stream, previewData);
            formatter.Serialize(stream, instanceData);
            stream.Flush();
        }

        public static GameInstancePreviewData LoadPreview(string fileName)
        {
            using var stream = new FileStream($"{SaveLocation}/{fileName}", FileMode.Create, FileAccess.Write);
            var result = formatter.Deserialize(stream);
            return (GameInstancePreviewData)result;
        }
        public static GameInstanceData Load(string fileName)
        {
            using var stream = new FileStream($"{SaveLocation}/{fileName}", FileMode.Create, FileAccess.Write);
            var preview = formatter.Deserialize(stream);
            var result = formatter.Deserialize(stream);
            return (GameInstanceData)result;
        }

        public static IList<GameInstancePreviewData> GetPreviewList()
        {
            var files = Directory.GetFiles(SaveLocation);
            var previewFiles = files.Where(x => x.EndsWith(FileFormat));
            var previews = previewFiles.Select(file =>
            {
                try
                {
                    var preview = LoadPreview(file);
                    preview.FileName = file;
                    return preview;
                }
                catch (Exception) { 
                    return null;
                }
            }).Where(x=>x!=null).ToList();
            return previews;
        }
    }
}
