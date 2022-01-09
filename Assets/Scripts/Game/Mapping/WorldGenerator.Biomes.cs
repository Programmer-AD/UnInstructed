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
        private const int requiredSticks = 40;
        private const int requiredIronOre = 20;
        private const int biomeSize = 5;

        private BiomeType[,] biomes;

        private void MakeBiomes()
        {
            biomes = new BiomeType[settings.Width / biomeSize, settings.Height / biomeSize];

            RectangleFor(0, 0, biomes.GetLength(0), biomes.GetLength(1), (x, y) =>
                {
                    biomes[x, y] = GetRandomBiom();
                });
            RectangleFor(0, 0, biomes.GetLength(0), biomes.GetLength(1), MakeBiom);
        }

        private BiomeType GetRandomBiom()
        {
            var value = random.Next(9);
            var result = value switch
            {
                >= 1 and < 5 => BiomeType.Forest,
                >= 5 => BiomeType.Cave,
                _ => BiomeType.Empty,
            };
            return result;
        }

        private void MakeBiom(int biomX, int biomY)
        {
            var biomType = biomes[biomX, biomY];
            switch (biomType)
            {
                case BiomeType.Forest:
                    MakeForestBiom(biomX, biomY);
                    return;
                case BiomeType.Cave:
                    MakeCaveBiom(biomX, biomY);
                    return;
            }
        }

        private (int x, int y) GetBiomStart(int biomX, int biomY)
            => (biomX * biomeSize, biomY * biomeSize);

        private bool IsBiomOfType(int biomX, int biomY, BiomeType type, bool defaultResult)
        {
            if (biomX < 0 || biomY < 0 || biomX >= biomes.GetLength(0) || biomY >= biomes.GetLength(1))
            {
                return defaultResult;
            }
            return biomes[biomX, biomY] == type;
        }
    }
}
