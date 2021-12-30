using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;

namespace Uninstructed.Game.Mapping
{
    public class WorldGenerator
    {
        private readonly GameObjectFactory factory;
        private GenerationSettings settings;
        private GameWorld world;

        public WorldGenerator(GameObjectFactory factory)
        {
            this.factory = factory;
        }

        public void Generate(GenerationSettings settings, GameWorld world)
        {
            this.settings = settings;
            this.world = world;

            InitWorld();
            GenerateMap();

            this.settings = null;
            this.world = null;
        }

        private void InitWorld()
        {
            world.MapName = settings.MapName;
            world.Map = new Map(settings.Width, settings.Height);
            world.Entities = new List<Entity>();
            world.DroppedItems = new List<Item>();
        }

        private void GenerateMap()
        {
            MakeSimpleMap();
        }

        private void MakeSimpleMap()
        {
            var simpleMap = new BlockType[settings.Width, settings.Height];

            MakeSpawn(simpleMap);
            MakeBorders(simpleMap);

            InstantinateSimpleMap(simpleMap);
        }

        private void MakeSpawn(BlockType[,] simpleMap)
        {
            const int spawnSize = 3;
            var middleX = settings.Width / 2;
            var middleY = settings.Height / 2;

            for (int by = 0; by < spawnSize; by++)
            {
                for (int bx = 0; bx < spawnSize; bx++)
                {
                    var x = middleX + bx - spawnSize / 2;
                    var y = middleY + by - spawnSize / 2;
                    simpleMap[x, y] = BlockType.Empty;
                }
            }
            AddPlayer(middleX, middleY);
        }

        private void AddPlayer(int x, int y)
        {
            var player = factory.Create(EntityType.Player);
            player.transform.position = new UnityEngine.Vector3(x, y);
            world.Entities.Add(player);
        }

        private void MakeBorders(BlockType[,] simpleMap)
        {
            var width = simpleMap.GetLength(0);
            var height = simpleMap.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                simpleMap[i, 0] = simpleMap[i, height - 1] = BlockType.BorderWall;
            }
            for (int i = 0; i < height; i++)
            {
                simpleMap[0, i] = simpleMap[width - 1, i] = BlockType.BorderWall;
            }
        }

        private void InstantinateSimpleMap(BlockType[,] simpleMap)
        {
            for (int y = 0; y < world.Map.Height; y++)
            {
                for (int x = 0; x < world.Map.Width; x++)
                {
                    var blockType = simpleMap[x, y];
                    if (blockType != BlockType.Empty)
                    {
                        world.Map[x, y] = factory.Create(blockType);
                    }
                }
            }
        }
    }
}
