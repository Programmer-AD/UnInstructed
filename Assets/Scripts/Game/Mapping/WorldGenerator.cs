using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Content.Enums;

namespace Uninstructed.Game.Mapping
{
    public partial class WorldGenerator
    {
        private static void RectangleFor(int x, int y, int width, int height, Action<int, int> action)
        {
            for (var dx = 0; dx < width; dx++)
            {
                for (var dy = 0; dy < height; dy++)
                {
                    action(x + dx, y + dy);
                }
            }
        }

        private readonly GameObjectFactory factory;
        private readonly Random random;

        private GenerationSettings settings;
        private GameWorld world;

        private BlockType[,] map;
        private readonly List<(EntityType entityType, int x, int y)> entities;
        private readonly List<(ItemType itemType, int count, int x, int y)> items;

        public WorldGenerator(GameObjectFactory factory)
        {
            this.factory = factory;

            entities = new();
            items = new();
            random = new();
        }

        public void Generate(GenerationSettings settings, GameWorld world)
        {
            this.settings = settings;
            this.world = world;
            world.MapName = settings.MapName;
            map = new BlockType[settings.Width, settings.Height];

            MakeWorld();
            InitWorld();

            items.Clear();
            entities.Clear();
            map = null;
            this.settings = null;
            this.world = null;
        }

        private void InitWorld()
        {
            world.MapName = settings.MapName;
            world.Map = new Map(settings.Width, settings.Height);

            RectangleFor(0, 0, settings.Width, settings.Height, (x, y) =>
            {
                var blockType = map[x, y];
                if (blockType != BlockType.Empty)
                {
                    world.Map[x, y] = factory.Create(blockType);
                }
            });

            foreach (var (entityType, x, y) in entities)
            {
                InitEntity(entityType, x, y);
            }

            foreach (var (itemType, count, x, y) in items)
            {
                InitItem(itemType, count, x, y);
            }
        }

        private void InitEntity(EntityType entityType, int x, int y)
        {
            var entity = factory.Create(entityType);
            entity.transform.position = new UnityEngine.Vector3(x, y);
        }

        private void InitItem(ItemType itemType, int count, int x, int y)
        {
            if (count <= 0)
            {
                return;
            }
            var item = factory.Create(itemType);
            if (count > item.MaxCount)
            {
                InitItem(itemType, count - item.MaxCount, x, y);
            }
            item.Count = count;
            item.transform.position = new UnityEngine.Vector3(x, y);
        }

        private void AddEntity(EntityType entityType, int x, int y)
        {
            entities.Add((entityType, x, y));
        }

        private void AddItem(ItemType itemType, int count, int x, int y)
        {
            items.Add((itemType, count, x, y));
        }

        private void FillMap(BlockType blockType, int x, int y, int width, int height)
        {
            RectangleFor(x, y, width, height,
                (x, y) => map[x, y] = blockType);
        }

        private (int x, int y) GetRandomPoint(int centreDistance)
        {
            int cx = settings.Width / 2, cy = settings.Height;
            int x, y;
            do
            {
                x = random.Next(1, settings.Width - 1);
                y = random.Next(1, settings.Height - 1);
            } while (Math.Abs(cx - x) + Math.Abs(cy - y) < centreDistance);
            return (x, y);
        }

        private void MakeWorld()
        {
            MakeBiomes();
            MakeSpawn();
            MakeLauncher();
            MakeBorders();

            FixRequiredItems();
            FixPlaces();
        }

        private void MakeLauncher()
        {
            const int launcherDistance = 15;
            (var x, var y) = GetRandomPoint(launcherDistance);
            map[x, y] = BlockType.Launcher;
        }

        private void MakeSpawn()
        {
            const int spawnSize = 3;

            var startX = (settings.Width - spawnSize) / 2;
            var startY = (settings.Height - spawnSize) / 2;

            FillMap(BlockType.Empty, startX, startY, spawnSize, spawnSize);
            AddEntity(EntityType.Player, startX + spawnSize / 2, startY + spawnSize / 2);
            AddStartItems(startX, startY, spawnSize, spawnSize);
        }

        private void AddStartItems(int x, int y, int width, int height)
        {
            AddItem(ItemType.Stone, 3, x, y);
            AddItem(ItemType.WoodStick, 2, x + width, y + height);
            AddItem(ItemType.CraftBench, 1, x + width, y);
        }

        private void MakeBorders()
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);
            for (var i = 0; i < width; i++)
            {
                map[i, 0] = map[i, height - 1] = BlockType.BorderWall;
            }
            for (var i = 0; i < height; i++)
            {
                map[0, i] = map[width - 1, i] = BlockType.BorderWall;
            }
        }

        private void FixRequiredItems()
        {
            var mapEnumerable = map.Cast<BlockType>();
            var itemEnumerable = items.Cast<(ItemType type, int count, int x, int y)>();

            var bushCount = mapEnumerable.Count(x => x == BlockType.Bush);
            var droppedSticks = itemEnumerable.Sum(x => x.type == ItemType.IronOre ? x.count : 0);
            var addSticks = requiredSticks - (5 * bushCount + droppedSticks);

            var ironOre = mapEnumerable.Count(x => x == BlockType.IronOre);
            var addIronOre = requiredIronOre - ironOre;

            (var x, var y) = GetRandomPoint(10);
            if (addSticks > 0)
            {
                AddItem(ItemType.WoodStick, addSticks, x, y);
            }
            if (addIronOre > 0)
            {
                AddItem(ItemType.IronOre, addIronOre, x, y);
            }

            map[x, y] = BlockType.Empty;
        }

        private void FixPlaces()
        {
            for (var i = 0; i < items.Count; i++)
            {
                var (itemType, count, x, y) = items[i];
                if (map[x, y] != BlockType.Empty)
                {
                    var (newX, newY) = FindNearestEmpty(x, y);
                    items[i] = (itemType, count, newX, newY);
                }
            }

            for (var i = 0; i < entities.Count; i++)
            {
                var (entityType, x, y) = entities[i];
                if (map[x, y] != BlockType.Empty)
                {
                    var (newX, newY) = FindNearestEmpty(x, y);
                    entities[i] = (entityType, newX, newY);
                }
            }
        }

        private (int x, int y) FindNearestEmpty(int x, int y, int maxRange = 15)
        {
            bool IsEmpty(int x, int y)
            {
                return x >= 0 && y >= 0 && x < settings.Width && y < settings.Height && map[x, y] == BlockType.Empty;
            }

            for (var range = 1; range < maxRange; range++)
            {
                var actions = 4 * range;
                int dx = 0, dy = -range;
                for (var i = 0; i < actions; i++)
                {
                    if (IsEmpty(x + dx, y + dy))
                    {
                        return (x + dx, y + dy);
                    }
                    switch (i / range)
                    {
                        case 0:
                            dx++;
                            dy++;
                            break;
                        case 1:
                            dx--;
                            dy++;
                            break;
                        case 2:
                            dx--;
                            dy--;
                            break;
                        case 3:
                            dx++;
                            dy--;
                            break;
                    }
                }
            }
            return (x, y);
        }
    }
}
