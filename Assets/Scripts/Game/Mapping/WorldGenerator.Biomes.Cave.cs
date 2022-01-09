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
        private void MakeCaveBiom(int biomX, int biomY)
        {
            bool IsNeighborCave(int dx, int dy)
            {
                return IsBiomOfType(biomX + dx, biomY + dy, BiomeType.Cave, true);
            }

            bool leftCave = IsNeighborCave(-1, 0),
                rightCave = IsNeighborCave(1, 0),
                upCave = IsNeighborCave(0, -1),
                downCave = IsNeighborCave(0, 1);

            (var x, var y) = GetBiomStart(biomX, biomY);

            var smallHalf = biomeSize / 2;
            var bigHalf = biomeSize / 2;

            void FillQuarter(bool right, bool down)
            {
                int sx = x + (right ? bigHalf : 0),
                    sy = y + (down ? bigHalf : 0);
                FillMap(BlockType.Stone, sx, sy, smallHalf, smallHalf);
            }

            void FillLine(bool vertical, bool secondPart)
            {
                int sx = x, sy = y, width = smallHalf, height = smallHalf;

                if (vertical)
                {
                    sx += smallHalf;
                    sy += secondPart ? bigHalf : 0;
                    width = 1 + bigHalf - smallHalf;
                }
                else
                {
                    sx += secondPart ? bigHalf : 0;
                    sy += smallHalf;
                    height = 1 + bigHalf - smallHalf;
                }
                FillMap(BlockType.Stone, sx, sy, width, height);
            }

            if (leftCave)
            {
                if (upCave)
                {
                    FillQuarter(false, false);
                }
                if (downCave)
                {
                    FillQuarter(false, true);
                }
                FillLine(false, false);
            }
            if (rightCave)
            {
                if (upCave)
                {
                    FillQuarter(true, false);
                }
                if (downCave)
                {
                    FillQuarter(true, true);
                }
                FillLine(false, true);
            }
            if (upCave)
            {
                FillLine(true, false);
            }
            if (downCave)
            {
                FillLine(true, true);
            }

            map[x + smallHalf, y + smallHalf] = BlockType.Stone;

            RectangleFor(x, y, biomeSize, biomeSize, (x, y) =>
            {
                if (random.Next(2) == 0)
                {
                    map[x, y] = BlockType.Stone;
                }
            });

            RectangleFor(x, y, biomeSize, biomeSize, (x, y) =>
            {
                if (map[x, y] == BlockType.Stone && random.Next(3) == 0)
                {
                    map[x, y] = BlockType.IronOre;
                }
                if (map[x,y]==BlockType.Empty && random.Next(6) == 0)
                {
                    AddItem(ItemType.Stone, random.Next(1, 3), x, y);
                }
            });
        }
    }
}
