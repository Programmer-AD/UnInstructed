using System;
using System.Collections.Generic;
using System.Linq;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands.Models;

namespace Uninstructed.Game.Player.Commands.Processors
{
    internal class PlayerCommandProcessor : ICommandProcessor
    {
        private readonly Entity entity;
        private readonly Dictionary<string, Func<string[], ProcessingResult>> commandProcessors;

        public PlayerCommandProcessor(Entity entity)
        {
            this.entity = entity;
            commandProcessors = MakeProcessors();
        }

        public ProcessingResult Process(Command command)
        {
            if (entity.Dead)
            {
                return ProcessingResult.Dead();
            }
            if (command.Args.Length == 0)
            {
                return ProcessingResult.Error("No command specified to call from player");
            }
            if (!commandProcessors.ContainsKey(command.Args[0]))
            {
                return ProcessingResult.Error("Unknown command to call from player");
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
                ["move"] = ProcessMove,
                ["rotate"] = ProcessRotate,
                ["select"] = ProcessSelect,
                ["use"] = ProcessUse,
                ["drop"] = ProcessDrop,
                ["interact"] = ProcessInteract,
            };
            return result;
        }

        private ProcessingResult ProcessMove(string[] args)
        {
            if (args.Length == 0)
            {
                return ProcessingResult.Error("No distance argument in player move");
            }
            if (float.TryParse(args[1], out var distance))
            {
                entity.SetMove(distance);
                return ProcessingResult.Ok();
            }
            return ProcessingResult.Error("Wrong distance value in player move");
        }

        private ProcessingResult ProcessRotate(string[] args)
        {
            var anglePos = 0;
            var to = false;
            if (args.Length == 2)
            {
                if (args[0] == "to")
                {
                    to = true;
                    anglePos++;
                }
                else
                {
                    return ProcessingResult.Error("Wrong additional param at player rotate");
                }
            }
            if (args.Length < anglePos + 1)
            {
                return ProcessingResult.Error("No angle argument in player rotate");
            }
            if (float.TryParse(args[anglePos], out var angle))
            {
                entity.SetRotate(angle, to);
                return ProcessingResult.Ok();
            }
            return ProcessingResult.Error("Wrong angle value in player rotate");
        }

        private ProcessingResult ProcessSelect(string[] args)
        {
            if (args.Length == 0)
            {
                return ProcessingResult.Error("No slotNumber argument in player select");
            }
            if (int.TryParse(args[0], out var slotNumber))
            {
                if (slotNumber >= 0 && slotNumber < entity.InventorySize)
                {
                    entity.SelectedInventorySlot = slotNumber;
                    return ProcessingResult.Ok();
                }
            }
            return ProcessingResult.Error("Wrong slotNumber value in player select");
        }

        private ProcessingResult ProcessUse(string[] args)
        {
            bool used;
            if (args.Length == 1)
            {
                if (args[0] == "onblock")
                {
                    used = entity.UseOnBlock();
                    return used ? ProcessingResult.Ok() : ProcessingResult.Error("Not looking at block to use on");
                }
                else
                {
                    return ProcessingResult.Error("Wrong additional param at player use");
                }
            }
            used = entity.UseItem();
            return used ? ProcessingResult.Ok() : ProcessingResult.Error("No item selected to use");
        }

        private ProcessingResult ProcessDrop(string[] args)
        {
            var droped = entity.Drop();
            return droped ? ProcessingResult.Ok() : ProcessingResult.Error("No item selected to drop");
        }

        private ProcessingResult ProcessInteract(string[] args)
        {
            var interacted = entity.Interact(args);
            return interacted ? ProcessingResult.Ok() : ProcessingResult.Error("Not looking at block to interact");
        }
    }
}
