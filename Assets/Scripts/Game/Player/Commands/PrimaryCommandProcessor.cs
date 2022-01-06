using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Main;
using Uninstructed.Game.Player.Commands.Models;
using Uninstructed.Game.Player.Commands.Processors;

namespace Uninstructed.Game.Player.Commands
{
    internal class PrimaryCommandProcessor : ICommandProcessor
    {
        private readonly Dictionary<CommandType, ICommandProcessor> commandProcessors;

        public PrimaryCommandProcessor(Entity player, Action startCallback, Action stopCallback)
        {
            commandProcessors = new Dictionary<CommandType, ICommandProcessor>
            {
                [CommandType.Unknown] = new UnknownCommandProcessor(),
                [CommandType.Get] = new GetCommandProcessor(player),
                [CommandType.Player] = new PlayerCommandProcessor(player),
                [CommandType.Work] = new WorkCommandProcessor(startCallback, stopCallback),
            };
        }

        public ProcessingResult Process(Command command)
        {
            if (commandProcessors.TryGetValue(command.Type, out var processor))
            {
                return processor.Process(command);
            }
            return ProcessingResult.Error("No command processor for this command type");
        }
    }
}
