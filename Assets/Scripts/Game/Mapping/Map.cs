using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Saving.Interfaces;
using Uninstructed.Game.Saving.Models;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            Blocks = memento.Blocks.Select(x => x!=null?factory.Load(x):null).ToArray();
        }

        public void InitPositions()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var block = this[x, y];
                    if (block != null)
                    {
                        block.transform.localPosition = new Vector3(x, y);
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
                Blocks = Blocks.Select(x => x != null ? x.Save() : null).ToArray(),
            };

            return memento;
        }
    }
}
