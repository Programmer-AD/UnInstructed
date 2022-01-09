using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Mapping
{
    partial class WorldGenerator
    {
        private void MakeForestBiom(int biomX, int biomY)
        {
            (var x, var y) = GetBiomStart(biomX, biomY);
            RectangleFor(x, y, biomeSize, biomeSize, (x, y) =>
            {
                if (random.Next(0, 4) == 0)
                {
                    map[x, y] = BlockType.Bush;
                }
            });

            RectangleFor(x, y, biomeSize, biomeSize, (x, y) => { 
                if (map[x, y] == BlockType.Empty && random.Next(6) == 0)
                {
                    AddItem(ItemType.WoodStick, random.Next(1, 4), x, y);
                }
            });
        }
    }
}
