using System.Linq;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;

namespace Uninstructed.Game.Mapping
{
    public class Map : ISaveable<MapData>
    {
        private Block[] Blocks { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map() { }
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Blocks = new Block[Width * Height];
        }

        public Block this[int x, int y]
        {
            get => Blocks[y * Width + x];
            set => Blocks[y * Width + x] = value;
        }

        public void Load(MapData memento, GameObjectFactory factory)
        {
            Width = memento.Width;
            Height = memento.Height;
            Blocks = memento.Blocks.Select(x => x != null ? factory.Load(x) : null).ToArray();
        }

        public void Init(Transform contentParent)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var block = this[x, y];
                    if (block != null)
                    {
                        block.transform.localPosition = new Vector3(x, y);
                        block.transform.SetParent(contentParent, true);
                    }
                }
            }
        }

        public MapData Save()
        {
            var memento = new MapData
            {
                Width = Width,
                Height = Height,
                Blocks = Blocks.Select(x => x != null && !x.Broken ? x.Save() : null).ToArray(),
            };

            return memento;
        }

        public void Optimize()
        {
            for (var i = 0; i < Blocks.Length; i++)
            {
                var block = Blocks[i];
                if (block != null && block.Broken)
                {
                    Blocks[i] = null;
                }
            }
        }
    }
}
