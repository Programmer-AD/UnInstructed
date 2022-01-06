using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processing
{
    internal class GetCommandProcessor : ICommandProcessor
    {
        private readonly Entity player;
        private readonly GameWorld world;
        private readonly Dictionary<string, Func<string[], ProcessingResult>> commandProcessors;

        public GetCommandProcessor(Entity player)
        {
            this.player = player;

            world = player.World;
            commandProcessors = MakeProcessors();
        }

        public ProcessingResult Process(Command command)
        {
            if (command.Args.Length == 0)
            {
                return ProcessingResult.Error("No command specified to call from get");
            }
            if (!commandProcessors.ContainsKey(command.Args[0]))
            {
                return ProcessingResult.Error("Unknown command to call from get");
            }
            var processor = commandProcessors[command.Args[0]];
            var args = command.Args.Skip(1).ToArray();
            var result = processor(args);
            return result;
        }

        private Dictionary<string, Func<string[], ProcessingResult>> MakeProcessors()
        {
            var result = new Dictionary<string, Func<string[], ProcessingResult>>()
            {
                ["player"] = ProcessPlayer,
                ["map"] = ProcessMap,
                ["list"] = ProcessList,
                ["info"] = ProcessInfo
            };
            return result;
        }

        private ProcessingResult ProcessPlayer(string[] args)
        {
            if (args.Length == 0)
            {
                return ProcessingResult.Error("No argument for get player");
            }
            var stringBuilder = new StringBuilder();
            switch (args[0])
            {
                case "position":
                    {
                        var position = player.transform.position;
                        stringBuilder.AppendPosition(position);
                    }
                    break;
                case "rotation":
                    {
                        var angle = player.transform.rotation.eulerAngles.x;
                        stringBuilder.Append(angle);
                    }
                    break;
                case "inventory":
                    {
                        stringBuilder.Append(player.InventorySize).AppendLine();
                        foreach (var item in player.Inventory)
                        {
                            stringBuilder.AppendShortInfo(item).AppendLine();
                        }
                    }
                    break;
                case "selected":
                    {
                        stringBuilder.Append(player.SelectedInventorySlot);
                    }
                    break;
                case "handitem":
                    {
                        stringBuilder.AppendLongInfo(player.HandItem);
                    }
                    break;
                default:
                    return ProcessingResult.Error("Wrong property to get from player");
            }
            return ProcessingResult.Ok(stringBuilder.ToString());

        }

        private ProcessingResult ProcessMap(string[] args)
        {
            if (args.Length is 2 or 3)
            {
                return ProcessingResult.Error("Wrong argumet count for get map");
            }

            var stringBuilder = new StringBuilder();
            int sx = 0, sy = 0, ex = world.Map.Width, ey = world.Map.Height;

            if (args.Length == 4)
            {
                if (int.TryParse(args[0], out sx) && int.TryParse(args[1], out sy) && int.TryParse(args[2], out ex) && int.TryParse(args[3], out ey))
                {
                    if (sx >= ex || sy >= ey)
                    {
                        return ProcessingResult.Error("Start point coordinates must be less than end point coordinates at get map");
                    }
                    if (sx < 0 || sy<0|| ex > world.Map.Width || ey > world.Map.Height)
                    {
                        return ProcessingResult.Error("Coordinates out of range at get map");
                    }
                }
                else
                {
                    return ProcessingResult.Error("Wrong borders format at get map");
                }
            }

            stringBuilder.Append(ex - sx).Append(' ').Append(ey - sy);
            if (args.Length == 1)
            {
                if (args[0] == "size")
                {
                    return ProcessingResult.Ok(stringBuilder.ToString());
                }
                return ProcessingResult.Error("Wrong argument for get map");
            }

            stringBuilder.AppendLine();
            for (int y = sy; y < ey; y++)
            {
                for (int x = sx; x < ex; x++)
                {
                    stringBuilder.AppendShortInfo(world.Map[x, y]);
                }
                stringBuilder.AppendLine();
            }
            return ProcessingResult.Ok(stringBuilder.ToString());
        }

        private ProcessingResult ProcessList(string[] args)
        {
            if (args.Length == 0)
            {
                return ProcessingResult.Error("No type of list in get list");
            }

            var stringBuilder = new StringBuilder();
            switch (args[0])
            {
                case "entity":
                    stringBuilder.Append(world.Entities.Count).AppendLine();
                    foreach (var entity in world.Entities)
                    {
                        stringBuilder.AppendShortInfo(entity).AppendLine();
                    }
                    break;
                case "item":
                    stringBuilder.Append(world.DroppedItems.Count).AppendLine();
                    foreach (var item in world.DroppedItems)
                    {
                        stringBuilder.AppendShortInfo(item, true).AppendLine();
                    }
                    break;
                default:
                    return ProcessingResult.Error("Wrong type of list in get list");
            }
            return ProcessingResult.Ok(stringBuilder.ToString());
        }

        private ProcessingResult ProcessInfo(string[] args)
        {
            if (args.Length == 0)
            {
                return ProcessingResult.Error("No argument for get info");
            }

            var stringBuilder = new StringBuilder();
            switch (args[0])
            {
                case "block":
                    {
                        if (args.Length < 3)
                        {
                            return ProcessingResult.Error("Wrong argument count at get info block");
                        }
                        if (int.TryParse(args[1], out var x) && int.TryParse(args[2], out var y))
                        {
                            if (x < 0 || y < 0 || x > world.Map.Width || y > world.Map.Height)
                            {
                                return ProcessingResult.Error("Block coordinates out of range at get info block");
                            }
                            var block = world.Map[x, y];
                            stringBuilder.AppendLongInfo(block);
                        }
                        else
                        {
                            return ProcessingResult.Error("Wrong block coordinates format at get info block");
                        }
                    }
                    break;
                case "entity":
                    {
                        if (args.Length < 2)
                        {
                            return ProcessingResult.Error("Wrong argument count at get info block");
                        }
                        if (int.TryParse(args[1], out var index))
                        {
                            if (index < 0 || index > world.Entities.Count )
                            {
                                return ProcessingResult.Error("Entity list index out of range at get info entity");
                            }
                            var entity = world.Entities[index];
                            stringBuilder.AppendLongInfo(entity);
                        }
                        else
                        {
                            return ProcessingResult.Error("Wrong entity list index format at get info entity");
                        }
                    }
                    break;
                case "player":
                    stringBuilder.AppendLongInfo(player);
                    break;
                default:
                    return ProcessingResult.Error("Wrong property to get info");
            }
            return ProcessingResult.Ok(stringBuilder.ToString());
        }
    }
}
